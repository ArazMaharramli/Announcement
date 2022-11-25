using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerRolesUpdated.Handlers;

public class UpdateUserRolesWhenManagerRolesUpdatedIntegrationEventHandler : INotificationHandler<ManagerRolesUpdatedIntegrationEvent>
{
    private readonly IUserManager _userManager;

    public UpdateUserRolesWhenManagerRolesUpdatedIntegrationEventHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ManagerRolesUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _userManager.RemoveAllRolesAsync(notification.ManagerId, cancellationToken);
        await _userManager.AddToRolesAsync(notification.ManagerId, notification.RoleNames);
    }
}

