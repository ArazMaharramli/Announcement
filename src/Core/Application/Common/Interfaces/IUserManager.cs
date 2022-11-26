using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<(Result Result, string UserId)> CreateUserAsync(string name, string userName, string password);
        Task<(Result Result, UserDTO User)> CreateUserAsync(string name, string tenantDomain, string phoneNumber, string email, string id = null, string profilePictureUrl = null);

        Task<Result> DeleteUserAsync(string userId);

        Task<IList<Claim>> GetUserClaimsAsync(string userId);
        Task ClearUserClaims(string userId, CancellationToken cancellationToken);
        Task AddUserClaims(string userId, List<string> claimNames);

        Task<string> GenerateEmailConfirmationToken(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);

        Task<(Result Result, UserDTO User)> ConfirmEmail(string userId, string confirmationCode);
        Task<Result> ResetPasswordAsync(string userId, string token, string newPassword);

        Task<UserDTO> FindByIdAsync(string userId);
        Task<(Result Result, UserDTO User)> FindByEmailAsync(string email, string tenantDomain);

        Task<(Result Result, UserDTO User)> LoginWithUserName(string userName, string password);

        Task AddToRolesAsync(string userId, List<string> roleNames);
        Task AddToRoleAsync(string userId, string roleName);
        Task RemoveFromRoleAsync(string userId, string roleName);
        Task RemoveAllRolesAsync(string userId, CancellationToken cancellationToken);

        Task<(string Token, string AccessTokenId)> CreateRefreshTokenAsync(string userId, CancellationToken cancellationToken);
        Task<(string Token, string AccessTokenId)> UpdateRefreshTokenAsync(string refreshToken, string accessTokenId, CancellationToken cancellationToken);
        Task<bool> InvalidateRefreshTokenAsync(string accessTokenId, CancellationToken cancellationToken);
    }
}
