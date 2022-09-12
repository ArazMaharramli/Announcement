using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Rooms.IntegrationEvents.Events;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Handlers
{
    public class SendEmailNotificationToAdminWhenRoomUpdatedIntegrationEventHandler : INotificationHandler<RoomUpdatedIntegrationEvent>
    {
        public Task Handle(RoomUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

