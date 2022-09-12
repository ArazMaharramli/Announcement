using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Events;

public class RoomDeclinedIntegrationEvent : IntegrationEvent, INotification
{
    public Room Room { get; set; }
    public string Notes { get; set; }

    public RoomDeclinedIntegrationEvent(Room room, string notes)
    {
        Room = room;
    }
}

