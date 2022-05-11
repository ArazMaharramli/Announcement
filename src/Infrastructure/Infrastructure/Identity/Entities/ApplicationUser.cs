using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        public ApplicationUser()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }
    }
}
