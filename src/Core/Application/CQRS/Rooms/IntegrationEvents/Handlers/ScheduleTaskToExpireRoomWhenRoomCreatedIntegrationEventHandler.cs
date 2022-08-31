using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Rooms.IntegrationEvents.Events;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Handlers
{
    public class ScheduleTaskToExpireRoomWhenRoomCreatedIntegrationEventHandler : INotificationHandler<RoomCreatedIntegrationEvent>
    {
        private readonly ITaskScheduler _taskScheduler;
        private readonly IDbContext _dbContext;

        public ScheduleTaskToExpireRoomWhenRoomCreatedIntegrationEventHandler(ITaskScheduler taskScheduler, IDbContext dbContext)
        {
            _taskScheduler = taskScheduler;
            _dbContext = dbContext;
        }

        public Task Handle(RoomCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

