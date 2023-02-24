﻿using MediatR;
using Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Domain.Events;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using Application.Common.Models.ConfigModels;
using Application.Common.Exceptions;
using Application.CQRS.Owners.IntegrationEvents.OwnerCreated;
//using Application.CQRS.Owners.IntegrationEvents.Events;

namespace Application.CQRS.Rooms.DomainEventHandlers.RoomCreated
{
    public class CreateOrAssignOwnerWhenRoomCreatedDomainEventHandler : INotificationHandler<RoomCreatedDomainEvent>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEventBusService _eventBusService;
        private readonly IUserManager _userManager;
        private readonly TenantInfo _tenant;

        public CreateOrAssignOwnerWhenRoomCreatedDomainEventHandler(IDbContext dbContext, ICurrentUserService currentUserService, IEventBusService eventBusService, IUserManager userManager, TenantInfo tenant)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _eventBusService = eventBusService;
            _userManager = userManager;
            _tenant = tenant;
        }

        public async Task Handle(RoomCreatedDomainEvent request, CancellationToken cancellationToken)
        {
            var room = request.Room;
            var userId = _currentUserService.UserId ?? Guid.NewGuid().ToString();
            var owner = await _dbContext.Owners.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            var ownerExisted = owner is not null && !_currentUserService.IsAuthenticated;

            if (owner is not null)
            {
                room.Owner = owner;
                await _dbContext.SaveEntitiesAsync(cancellationToken);
                return;
            }

            if (!ownerExisted)
            {
                if (!_currentUserService.IsAuthenticated)
                {
                    owner = await _dbContext.Owners.FirstOrDefaultAsync(x => x.Email == room.Contact.Email, cancellationToken);
                    var res = await _userManager.FindByEmailAsync(room.Contact.Email, _tenant.Domain);

                    if (res.Result.Succeeded)
                    {
                        userId = res.User.Id;
                    }
                    else
                    {
                        var resp = await _userManager.CreateUserAsync(
                            name: room.Contact.Name,
                            tenantDomain: _tenant.Domain.ToLower(),
                            phoneNumber: room.Contact.Phone,
                            email: room.Contact.Email,
                            id: userId
                        );
                        if (!resp.Result.Succeeded)
                        {
                            throw new BadRequestException(string.Join(';', resp.Result.Errors));
                        }
                    }

                }

                owner = new Owner(userId, room.Contact.Name, room.Contact.Email, room.Contact.Phone);
                _dbContext.Owners.Add(owner);
                _eventBusService.AddEvent(new OwnerCreatedIntegrationEvent(owner.Id, owner.Name, owner.Email, owner.Phone));
            }

            room.Owner = owner;

            await _dbContext.SaveEntitiesAsync(cancellationToken);
        }
    }
}

