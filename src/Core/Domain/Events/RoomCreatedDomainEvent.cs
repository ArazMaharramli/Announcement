using System;
using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class RoomCreatedDomainEvent : INotification
    {
        public RoomCreatedDomainEvent(Room room)
        {
            Room = room;
        }

        public Room Room { get; set; }
    }
}

