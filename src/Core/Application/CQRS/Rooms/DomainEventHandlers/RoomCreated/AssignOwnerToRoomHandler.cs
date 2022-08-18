using MediatR;
using Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Domain.Events;

namespace Application.CQRS.Rooms.DomainEventHandlers.RoomCreated
{
    public class AssignOwnerToRoomHandler : INotificationHandler<RoomCreatedDomainEvent>
    {
        private readonly IDbContext _dbContext;

        public AssignOwnerToRoomHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Handle(RoomCreatedDomainEvent request, CancellationToken cancellationToken)
        {
            request.Room.Name = "Name from event Handler";
            return Task.CompletedTask;
        }
    }
}

