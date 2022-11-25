using System;
using System.Collections.Generic;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerClaimsUpdated;

public class ManagerClaimsUpdatedIntegrationEvent : IntegrationEvent, INotification
{
    public string ManagerId { get; set; }
    public List<string> ClaimNames { get; set; }

    public ManagerClaimsUpdatedIntegrationEvent(string managerId, List<string> claimNames)
    {
        ManagerId = managerId;
        ClaimNames = claimNames;
    }
}

