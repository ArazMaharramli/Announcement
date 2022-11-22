using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Roles.RoleCreated.IntegrationEvents;
using MediatR;

namespace Application.CQRS.Roles.IntegrationEvents.RoleCreated.Handlers;

public class CreateApplicationRoleWhenRoleCreatedIntegrationEventHandler : INotificationHandler<RoleCreatedIntegrationEvent>
{
    private readonly IRoleManager _roleManager;

    public CreateApplicationRoleWhenRoleCreatedIntegrationEventHandler(IRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task Handle(RoleCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var result = await _roleManager.CreateAsync(notification.Id, notification.Name, notification.Claims);
        if (!result.Result.Succeeded)
        {

        }
    }
}

