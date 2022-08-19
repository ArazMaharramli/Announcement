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
        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
        Task<Result> CreateUserAsync(string userName, string phoneNumber, string email, string id = null);

        Task<Result> DeleteUserAsync(string userId);
        Task<IList<Claim>> GetUserClaimsAsync(string userId);

        Task<string> GenerateEmailConfirmationToken(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);

        Task<(Result Result, UserDTO User)> ConfirmEmail(string userId, string confirmationCode);
        Task<Result> ResetPasswordAsync(string userId, string token, string newPassword);

        Task<UserDTO> FindByIdAsync(string userId);
        Task<UserDTO> FindByEmailAsync(string email);

        Task<(Result Result, UserDTO User)> LoginWithUserName(string userName, string password);


        Task<(string Token, string AccessTokenId)> CreateRefreshTokenAsync(string userId, CancellationToken cancellationToken);
        Task<(string Token, string AccessTokenId)> UpdateRefreshTokenAsync(string refreshToken, string accessTokenId, CancellationToken cancellationToken);
        Task<bool> InvalidateRefreshTokenAsync(string accessTokenId, CancellationToken cancellationToken);

    }
}
