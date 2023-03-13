using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Events;

public class RoomUpdatedByOwnerIntegrationEvent : IntegrationEvent, INotification
{
    public Room Room { get; set; }

    public RoomUpdatedByOwnerIntegrationEvent(Room room)
    {
        Room = room;
    }
}