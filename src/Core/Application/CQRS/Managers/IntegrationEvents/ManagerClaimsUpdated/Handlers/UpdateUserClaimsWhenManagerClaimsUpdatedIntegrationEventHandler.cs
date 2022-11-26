using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerClaimsUpdated.Handlers;

public class UpdateUserClaimsWhenManagerClaimsUpdatedIntegrationEventHandler : INotificationHandler<ManagerClaimsUpdatedIntegrationEvent>
{
    private readonly IUserManager _userManager;

    public UpdateUserClaimsWhenManagerClaimsUpdatedIntegrationEventHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ManagerClaimsUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        await _userManager.ClearUserClaims(notification.ManagerId, cancellationToken);

        if (notification.ClaimNames is not null && notification.ClaimNames.Any())
        {
            await _userManager.AddUserClaims(notification.ManagerId, notification.ClaimNames);
        }
    }
}

