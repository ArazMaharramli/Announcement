using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }

        public DateTime RegisteredAt { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        public ApplicationUser()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public ApplicationUser(string name, string profilePictureUrl = null) : this()
        {
            Name = name.Trim();
            ProfilePictureUrl = profilePictureUrl;
        }

        public ApplicationUser(string id, string name, string userName, string email, string phoneNumber, string profilePictureUrl = null)
            : this(name, profilePictureUrl)
        {
            Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
