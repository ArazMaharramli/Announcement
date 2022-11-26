using System;
using Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerRecovered;

public class ManagerRecoveredIntegrationEvent : IntegrationEvent, INotification
{
    public string Id { get; set; }
    public List<string> Roles { get; set; }
    public List<string> ClaimNames { get; set; }

    public ManagerRecoveredIntegrationEvent(string id, List<string> roles, List<string> claims)
    {
        Id = id;
        Roles = roles;
        ClaimNames = claims;
    }
}
