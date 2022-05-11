using System;

namespace Infrastructure.Identity.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string AccessTokenId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool Expired { get; set; }
        public bool Invalidated { get; set; }
        public DateTime UpdateDate { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
