using System;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Events;

public class RoomCreatedIntegrationEvent : IntegrationEvent, INotification
{
    public Room Room { get; set; }

    public RoomCreatedIntegrationEvent(Room room)
    {
        Room = room;
    }
}
