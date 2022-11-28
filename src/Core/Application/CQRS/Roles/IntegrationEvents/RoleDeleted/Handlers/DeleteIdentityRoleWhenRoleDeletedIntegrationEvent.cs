using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Roles.IntegrationEvents.RoleDeleted.Handlers;

public class DeleteIdentityRoleWhenRoleDeletedIntegrationEvent : INotificationHandler<RoleDeletedIntegrationEvent>
{
    private readonly IRoleManager _roleManager;

    public DeleteIdentityRoleWhenRoleDeletedIntegrationEvent(IRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task Handle(RoleDeletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _roleManager.DeleteRole(notification.RoleId, cancellationToken);
    }
}

