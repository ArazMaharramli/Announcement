using MediatR;
using Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Domain.Events;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;

namespace Application.CQRS.Rooms.DomainEventHandlers.RoomCreated
{
    public class CreateOrAssignOwnerWhenRoomCreatedDomainEventHandler : INotificationHandler<RoomCreatedDomainEvent>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrAssignOwnerWhenRoomCreatedDomainEventHandler(IDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
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
            if (owner is null)
            {
                var userId = Guid.NewGuid().ToString();
                owner = new Owner(userId, room.Contact.Name, room.Contact.Email, room.Contact.Phone);
            }

            owner.AddRoom(room);
            _dbContext.Owners.Add(owner);
            await _dbContext.SaveEntitiesAsync(cancellationToken);
        }
    }
}

