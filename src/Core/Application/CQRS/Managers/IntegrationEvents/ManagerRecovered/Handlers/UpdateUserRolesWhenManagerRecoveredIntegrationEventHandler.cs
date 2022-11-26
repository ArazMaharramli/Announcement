using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerRecovered.Handlers;

public class UpdateUserRolesWhenManagerRecoveredIntegrationEventHandler : INotificationHandler<ManagerRecoveredIntegrationEvent>
{
    private readonly IUserManager _userManager;

    public UpdateUserRolesWhenManagerRecoveredIntegrationEventHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ManagerRecoveredIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _userManager.AddToRolesAsync(notification.Id, notification.Roles);
    }
}