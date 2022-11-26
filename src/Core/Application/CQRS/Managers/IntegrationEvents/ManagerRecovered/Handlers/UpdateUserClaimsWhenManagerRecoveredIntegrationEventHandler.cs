using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerRecovered.Handlers;

public class UpdateUserClaimsWhenManagerRecoveredIntegrationEventHandler : INotificationHandler<ManagerRecoveredIntegrationEvent>
{
    private readonly IUserManager _userManager;

    public UpdateUserClaimsWhenManagerRecoveredIntegrationEventHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ManagerRecoveredIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (notification.ClaimNames is not null && notification.ClaimNames.Any())
        {
            await _userManager.AddUserClaims(notification.Id, notification.ClaimNames);
        }
    }
}

