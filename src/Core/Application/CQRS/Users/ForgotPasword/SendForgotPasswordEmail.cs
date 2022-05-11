using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.CQRS.Users.ForgotPasword
{
    public class SendForgotPasswordEmail : INotification
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }

        public class Handler : INotificationHandler<SendForgotPasswordEmail>
        {
            private readonly IEmailService _emailService;
            private readonly StaticUrls _staticUrls;
            public Handler(IEmailService emailService, IOptions<StaticUrls> staticUrls)
            {
                _emailService = emailService;
                _staticUrls = staticUrls.Value;
            }

            public async Task Handle(SendForgotPasswordEmail notification, CancellationToken cancellationToken)
            {
                var link = string.Format(_staticUrls.ForgotPassword, notification.UserId, notification.Code);
                await _emailService.SendEmailAsync(notification.Email, "Reset Password", link);
            }
        }
    }
}
