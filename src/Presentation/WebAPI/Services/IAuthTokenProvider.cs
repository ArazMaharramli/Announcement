using System.Collections.Generic;
using System.Security.Claims;

namespace WebAPI.Services
{
    public interface IAuthTokenProvider
    {
        string CreateToken(string userId, IEnumerable<Claim> userClaims, string tokenId);

        string GetTokenId(string token);
        string GetUserId(string token);
    }
}
