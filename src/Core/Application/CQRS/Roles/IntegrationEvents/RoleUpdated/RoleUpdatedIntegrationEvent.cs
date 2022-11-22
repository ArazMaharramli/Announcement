using System;
using System.Collections.Generic;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Roles.IntegrationEvents.RoleUpdated;

public class RoleUpdatedIntegrationEvent : IntegrationEvent, INotification
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public List<string> Claims { get; private set; }

    public RoleUpdatedIntegrationEvent(string id, string name, List<string> claims)
    {
        Id = id;
        Name = name;
        Claims = claims;
    }

}

