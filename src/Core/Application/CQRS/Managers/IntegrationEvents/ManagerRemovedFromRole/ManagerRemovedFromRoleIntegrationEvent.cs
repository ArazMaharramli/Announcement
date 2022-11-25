using System;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerRemovedFromRole;

public class ManagerRemovedFromRoleIntegrationEvent : IntegrationEvent, INotification
{
    public string ManagerId { get; set; }
    public string RoleName { get; set; }

    public ManagerRemovedFromRoleIntegrationEvent(string managerId, string roleName)
    {
        ManagerId = managerId;
        RoleName = roleName;
    }
}

