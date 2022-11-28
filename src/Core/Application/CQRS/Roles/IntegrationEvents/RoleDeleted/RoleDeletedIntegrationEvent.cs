using System;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Roles.IntegrationEvents.RoleDeleted;

public class RoleDeletedIntegrationEvent : IntegrationEvent, INotification
{
    public string RoleId { get; set; }

    public RoleDeletedIntegrationEvent(string roleId)
    {
        RoleId = roleId;
    }
}

