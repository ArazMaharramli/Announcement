using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Events
{
    public class RoomUpdatedIntegrationEvent : IntegrationEvent, INotification
    {
        public Room Room { get; set; }

        public RoomUpdatedIntegrationEvent(Room room)
        {
            Room = room;
        }
    }
}

