using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Owners.IntegrationEvents.Events;
using MediatR;

namespace Application.CQRS.Owners.IntegrationEvents.Handlers
{
    public class SendEmailWhenOwnerUserCreatedIntegrationEventHandler : INotificationHandler<OwnerUserCreatedIntegrationEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserManager _userManager;
        public SendEmailWhenOwnerUserCreatedIntegrationEventHandler(IEmailService emailService, IUserManager userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task Handle(OwnerUserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(notification.UserId);
            var token = await _userManager.GenerateEmailConfirmationToken(notification.UserId);

            await _emailService.SendEmailAsync(user.Email, "Confirm Account", token);
        }
    }
}

