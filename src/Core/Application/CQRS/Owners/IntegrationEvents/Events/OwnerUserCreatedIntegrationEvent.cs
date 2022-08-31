using System;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Owners.IntegrationEvents.Events
{
    public class OwnerUserCreatedIntegrationEvent : IntegrationEvent, INotification
    {
        public Owner Owner { get; set; }

        public OwnerUserCreatedIntegrationEvent(Owner owner)
        {
            Owner = owner;
        }
    }
}

