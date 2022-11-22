using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Roles.IntegrationEvents.RoleUpdated.Handlers;

public class UpdateApplicationRoleWhenRoleUpdatedIntegrationEventHandler : INotificationHandler<RoleUpdatedIntegrationEvent>
{
    private readonly IRoleManager _roleManager;

    public UpdateApplicationRoleWhenRoleUpdatedIntegrationEventHandler(IRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task Handle(RoleUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var identityRoleUpdateSucceeded = await _roleManager.UpdateByIdAsync(notification.Id, notification.Name);
        var identityRoleClaims = await _roleManager.GetClaims(notification.Name);

        foreach (var item in identityRoleClaims)
        {
            var removeIdentityClaimResult = await _roleManager.RemoveOneRoleClaim(notification.Id, item);
        }

        foreach (var item in notification.Claims)
        {
            var addIdentityClaimResult = await _roleManager.AddOneRoleClaim(notification.Id, item);
        }
    }
}

