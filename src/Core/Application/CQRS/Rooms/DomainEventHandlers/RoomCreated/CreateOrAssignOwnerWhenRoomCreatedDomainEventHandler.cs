using MediatR;
using Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Domain.Events;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using Application.CQRS.Owners.Events.IntegrationEvents;
//using Application.CQRS.Owners.IntegrationEvents.Events;

namespace Application.CQRS.Rooms.DomainEventHandlers.RoomCreated
{
    public class CreateOrAssignOwnerWhenRoomCreatedDomainEventHandler : INotificationHandler<RoomCreatedDomainEvent>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEventBusService _eventBusService;

        public CreateOrAssignOwnerWhenRoomCreatedDomainEventHandler(IDbContext dbContext, ICurrentUserService currentUserService, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _eventBusService = eventBusService;
        }

        public async Task Handle(RoomCreatedDomainEvent request, CancellationToken cancellationToken)
        {
            var room = request.Room;
            if (!string.IsNullOrWhiteSpace(_currentUserService.UserId))
            {
                room.OwnerId = _currentUserService.UserId;
                return;
            }

            var owner = await _dbContext.Owners.FirstOrDefaultAsync(x => x.Email == room.Contact.Email, cancellationToken);
            var ownerExisted = owner is not null;
            if (owner is null)
            {
                var userId = Guid.NewGuid().ToString();
                owner = new Owner(userId, room.Contact.Name, room.Contact.Email, room.Contact.Phone);
                _dbContext.Owners.Add(owner);
                _eventBusService.AddEvent(new OwnerCreatedIntegrationEvent(owner));
            }

            owner.AddRoom(room);
            if (ownerExisted)
            {
                _dbContext.Owners.Update(owner);
            }

            await _dbContext.SaveEntitiesAsync(cancellationToken);
        }
    }
}

