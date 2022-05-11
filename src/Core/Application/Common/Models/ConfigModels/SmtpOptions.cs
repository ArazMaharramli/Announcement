using System;
namespace Application.Common.Models.ConfigModels
{
    public class SmtpOptions
    {
        public string SenderName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
    }
}
