using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerCreated.Handlers;

public class SendWelcomesEmailToManagerWhenManagerCreatedIntegrationEventHandler : INotificationHandler<ManagerCreatedIntegrationEvent>
{
    private readonly IEmailService _emailService;
    private readonly TenantInfo _tenant;

    public SendWelcomesEmailToManagerWhenManagerCreatedIntegrationEventHandler(IEmailService emailService, TenantInfo tenant)
    {
        _emailService = emailService;
        _tenant = tenant;
    }

    public async Task Handle(ManagerCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var body = string.Format(_tenant.EmailTemplates.WelcomeToNewManager.Body, notification.Name);
        await _emailService.SendEmailAsync(notification.Email, _tenant.EmailTemplates.WelcomeToNewManager.Subject, body);
    }
}

