using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Common;
using FluentValidation.Results;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services
{
    public class UserManagerService : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTimeService _dateTimeService;

        public UserManagerService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IDateTimeService dateTimeService)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _dateTimeService = dateTimeService;
        }

        public async Task<(Result Result, UserDTO User)> ConfirmEmail(string userId, string confirmationCode)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, confirmationCode);
            return (result.ToApplicationResult(), user.ToApplicationUserDTO());
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return await DeleteUserAsync(user);
            }
            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<UserDTO> FindByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.ToApplicationUserDTO();
        }

        public async Task<UserDTO> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user.ToApplicationUserDTO();
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<Result> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.ToApplicationResult();
        }


        public async Task<string> GenerateEmailConfirmationToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }


        public async Task<IList<Claim>> GetUserClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<(Result Result, UserDTO User)> LoginWithUserName(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                throw new NotFoundException(nameof(ApplicationUser), userName);
            }
            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return (Result.Failure(new List<string> { "Password is not valid" }), null);
            }
            if (user.LockoutEnabled && user.LockoutEnd > _dateTimeService.Now)
            {
                return (Result.Failure(new List<string> { "You are locked out." }), null);
            }
            return (Result.Success(), user.ToApplicationUserDTO());
        }

        public async Task<(string Token, string AccessTokenId)> CreateRefreshTokenAsync(string userId, CancellationToken cancellationToken)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                CreateDate = _dateTimeService.Now,
                ExpirationDate = _dateTimeService.Now.AddMonths(6),

                Expired = false,
                Invalidated = false,

                AccessTokenId = Guid.NewGuid().ToString(),
                UserId = userId,
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return (refreshToken.Token, refreshToken.AccessTokenId);
        }

        public async Task<bool> InvalidateRefreshTokenAsync(string accessTokenId, CancellationToken cancellationToken)
        {
            var token = await _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.AccessTokenId == accessTokenId);
            if (token is null)
            {
                throw new NotFoundException(nameof(RefreshToken), $"JwtId {accessTokenId}");
            }
            _dbContext.RefreshTokens.Remove(token);
            return (await _dbContext.SaveChangesAsync(cancellationToken)) > 0;
        }

        public async Task<bool> InvalidateUserTokensAsync(string userId, CancellationToken cancellationToken)
        {
            var tokens = await _dbContext.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();
            _dbContext.RefreshTokens.RemoveRange(tokens);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<(string Token, string AccessTokenId)> UpdateRefreshTokenAsync(string token, string accessTokenId, CancellationToken cancellationToken)
        {
            var refreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == token && x.AccessTokenId == accessTokenId);
            if (refreshToken is null)
            {
                throw new NotFoundException(nameof(RefreshToken), token);
            }
            if (!IsValidToken(refreshToken) || refreshToken.AccessTokenId != accessTokenId)
            {
                throw new ValidationException(new List<ValidationFailure> { new ValidationFailure("RefreshToken", "Invalid Token") });
            }

            _dbContext.RefreshTokens.Remove(refreshToken);

            var newRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                AccessTokenId = Guid.NewGuid().ToString(),
                CreateDate = _dateTimeService.Now,
                Expired = false,
                ExpirationDate = _dateTimeService.Now.AddMonths(6),
                UserId = refreshToken.UserId
            };

            _dbContext.RefreshTokens.Add(newRefreshToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return (newRefreshToken.Token, newRefreshToken.AccessTokenId);
        }

        #region MyRegion
        private bool IsValidToken(RefreshToken token)
        {
            return !(token.Expired || token.Invalidated);
        }
        #endregion
    }
}
