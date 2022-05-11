using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.CQRS.Users
{
    public class SendEmailConfirmationCode : INotification
    {
        public string UserId { get; set; }
        public string UserType { get; set; }

        public class SendEmailConfirmationCodeHandler : INotificationHandler<SendEmailConfirmationCode>
        {
            private readonly IUserManager _userManager;
            private readonly IEmailService _emailService;
            private readonly StaticUrls _staticUrls;

            public SendEmailConfirmationCodeHandler(IUserManager userManager, IEmailService emailService, IOptions<StaticUrls> staticUrls)
            {
                _userManager = userManager;
                _emailService = emailService;
                _staticUrls = staticUrls.Value;
            }

            public async Task Handle(SendEmailConfirmationCode notification, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(notification.UserId);
                var confirmationCode = await _userManager.GenerateEmailConfirmationToken(user.Id);
                var link = string.Format(_staticUrls.ConfirmEmail, notification.UserId, confirmationCode, notification.UserType);
                await _emailService.SendEmailAsync(user.Email, "Confrm Email", link);
            }
        }
    }
}
