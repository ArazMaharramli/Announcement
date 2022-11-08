using System;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Owners.Events.IntegrationEvents
{
    public class OwnerCreatedIntegrationEvent : IntegrationEvent, INotification
    {
        public Owner Owner { get; set; }

        public OwnerCreatedIntegrationEvent(Owner owner)
        {
            Owner = owner;
        }
    }
}

