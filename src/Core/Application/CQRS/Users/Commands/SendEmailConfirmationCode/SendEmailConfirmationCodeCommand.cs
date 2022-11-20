using System;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Commands.SendEmailConfirmationCode
{
    public class SendEmailConfirmationCodeCommand : INotification
    {
        public string UserId { get; set; }

        public class Handler : INotificationHandler<SendEmailConfirmationCodeCommand>
        {
            private readonly IEmailService _emailService;
            private readonly IUserManager _userManager;
            private readonly EmailTemplates _emailTemplates;
            private readonly StaticUrls _staticUrls;

            public Handler(IEmailService emailService, IUserManager userManager, TenantInfo tenant, StaticUrls staticUrls)
            {
                _emailService = emailService;
                _userManager = userManager;
                _emailTemplates = tenant.EmailTemplates;
                _staticUrls = staticUrls;
            }

            public async Task Handle(SendEmailConfirmationCodeCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    var token = await _userManager.GenerateEmailConfirmationToken(user.Id);
                    var link = string.Format(_staticUrls.ConfirmEmail, user.Id, token);

                    await _emailService.SendEmailAsync(user.Email, _emailTemplates.UserEmailConfirmation.Subject, string.Format(_emailTemplates.UserEmailConfirmation.Body, user.Name, token, link));
                }
            }
        }
    }
}

