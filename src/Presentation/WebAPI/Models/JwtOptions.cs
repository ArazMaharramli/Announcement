using System;
namespace WebAPI.Models
{
    public class JwtOptions
    {
        public string SecurityKey { get; set; }
        public TimeSpan LifeTime { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }

        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
    }
}
