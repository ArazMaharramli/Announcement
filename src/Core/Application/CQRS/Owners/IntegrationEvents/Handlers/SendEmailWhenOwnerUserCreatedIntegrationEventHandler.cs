using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Owners.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.CQRS.Owners.IntegrationEvents.Handlers
{
    public class SendEmailWhenOwnerUserCreatedIntegrationEventHandler : INotificationHandler<OwnerUserCreatedIntegrationEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserManager _userManager;
        private readonly EmailTemplates _emailTemplates;
        private readonly StaticUrls _staticUrls;

        public SendEmailWhenOwnerUserCreatedIntegrationEventHandler(IEmailService emailService, IUserManager userManager, EmailTemplates emailTemplates, StaticUrls staticUrls)
        {
            _emailService = emailService;
            _userManager = userManager;
            _emailTemplates = emailTemplates;
            _staticUrls = staticUrls;
        }

        public async Task Handle(OwnerUserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var owner = notification.Owner;
            var user = await _userManager.FindByIdAsync(owner.Id);

            var token = await _userManager.GenerateEmailConfirmationToken(owner.Id);
            var link = string.Format(_staticUrls.ConfirmEmail, owner.Id, token);

            await _emailService.SendEmailAsync(user.Email, _emailTemplates.UserEmailConfirmation.Subject, string.Format(_emailTemplates.UserEmailConfirmation.Body, owner.Name, token, link));
        }
    }
}

