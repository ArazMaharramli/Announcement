using System;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Users.Events.IntegrationEvents
{
    public class UserRegisteredIntegrationEvent : IntegrationEvent, INotification
    {
        public UserDTO User { get; set; }

        public UserRegisteredIntegrationEvent(UserDTO user)
        {
            User = user;
        }
    }
}

