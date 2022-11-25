using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerDeleted.Handlers;

public class RemoveUserFromRoleWhenManagerDeletedIntegrationEventHandler : INotificationHandler<ManagerDeletedIntegrationEvent>
{
    private readonly IUserManager _userManager;

    public RemoveUserFromRoleWhenManagerDeletedIntegrationEventHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ManagerDeletedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _userManager.RemoveAllRolesAsync(notification.Id, cancellationToken);
    }
}

