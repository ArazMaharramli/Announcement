using System;
using System.Collections.Generic;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Roles.RoleCreated.IntegrationEvents
{
    public class RoleCreatedIntegrationEvent : IntegrationEvent, INotification
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public List<string> Claims { get; private set; }

        public RoleCreatedIntegrationEvent(string id, string name, List<string> claims)
        {
            Id = id;
            Name = name;
            Claims = claims;
        }
    }
}

