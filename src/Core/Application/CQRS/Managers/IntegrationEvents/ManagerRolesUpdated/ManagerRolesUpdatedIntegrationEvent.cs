using System;
using System.Collections.Generic;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerRolesUpdated;

public class ManagerRolesUpdatedIntegrationEvent : IntegrationEvent, INotification
{
    public ManagerRolesUpdatedIntegrationEvent(string managerId, List<string> roleNames)
    {
        ManagerId = managerId;
        RoleNames = roleNames;
    }

    public string ManagerId { get; set; }
    public List<string> RoleNames { get; set; }
}

