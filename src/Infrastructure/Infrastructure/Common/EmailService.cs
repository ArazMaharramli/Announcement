using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Common
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtpOptions;

        public EmailService(TenantInfo tenantInfo)
        {
            _smtpOptions = tenantInfo.SmtpOptions;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_smtpOptions.SenderName, _smtpOptions.Email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            bodyBuilder.TextBody = message;

            emailMessage.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.Auto);
                client.Authenticate(_smtpOptions.Email, _smtpOptions.Password);
                client.Send(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
