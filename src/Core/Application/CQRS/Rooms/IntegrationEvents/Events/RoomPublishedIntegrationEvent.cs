using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Events
{
    public class RoomPublishedIntegrationEvent : IntegrationEvent, INotification
    {
        public Room Room { get; set; }

        public RoomPublishedIntegrationEvent(Room room)
        {
            Room = room;
        }
    }
}

