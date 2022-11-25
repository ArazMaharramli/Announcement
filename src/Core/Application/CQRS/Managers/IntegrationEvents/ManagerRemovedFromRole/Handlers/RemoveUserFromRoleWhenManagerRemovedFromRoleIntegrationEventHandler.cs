using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerRemovedFromRole.Handlers;

public class RemoveUserFromRoleWhenManagerRemovedFromRoleIntegrationEventHandler : INotificationHandler<ManagerRemovedFromRoleIntegrationEvent>
{
    private readonly IUserManager _userManager;

    public RemoveUserFromRoleWhenManagerRemovedFromRoleIntegrationEventHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ManagerRemovedFromRoleIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _userManager.RemoveFromRoleAsync(notification.ManagerId, notification.RoleName);
    }
}

