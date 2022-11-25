using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerCreated.Handlers;

public class CreateUserWhenManagerCreatedIntegrationEventHandler : INotificationHandler<ManagerCreatedIntegrationEvent>
{
    private readonly IUserManager _userManager;
    private readonly TenantInfo _tenant;

    public CreateUserWhenManagerCreatedIntegrationEventHandler(IUserManager userManager, TenantInfo tenant)
    {
        _userManager = userManager;
        _tenant = tenant;
    }

    public async Task Handle(ManagerCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (!notification.CreateUser)
        {
            return;
        }

        var res = await _userManager.CreateUserAsync(notification.Name, _tenant.Domain, notification.Phone, notification.Email, notification.Id);

        if (res.Result.Succeeded)
        {
            await _userManager.AddToRolesAsync(res.User.Id, notification.RoleNames);
        }

        //else => revert all operations back
    }
}