using System;
using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class RoomCreatedDomainEvent : INotification
    {
        public Room Room { get; set; }

        public RoomCreatedDomainEvent(Room room)
        {
            Room = room;
        }

    }
}

