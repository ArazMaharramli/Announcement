using System;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Owners.IntegrationEvents.Events
{
    public class OwnerUserCreatedIntegrationEvent : IntegrationEvent, INotification
    {
        public string UserId { get; set; }

        public OwnerUserCreatedIntegrationEvent(string userId)
        {
            UserId = userId;
        }
    }
}

