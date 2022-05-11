using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Application.Common.Exceptions;
using Common;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class JwtTokenProvider : IAuthTokenProvider
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly JwtOptions _jwtOptions;


        public JwtTokenProvider(IDateTimeService dateTimeService, IOptions<JwtOptions> jwtOptions)
        {
            _dateTimeService = dateTimeService;
            _jwtOptions = jwtOptions.Value;
        }

        public string CreateToken(string userId, IEnumerable<Claim> userClaims, string tokenId)
        {

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId)
            };

            if (userClaims != null)
            {
                authClaims.AddRange(userClaims);
            }


            var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey);
            var expirationDate = _dateTimeService.Now.Add(_jwtOptions.LifeTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                IssuedAt = _dateTimeService.Now,
                NotBefore = _dateTimeService.Now,
                Expires = expirationDate,
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetTokenId(string token)
        {
            var principals = GetPrincipalFromToken(token);

            if (principals is not null)
            {
                return principals.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            }

            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("token", "Xətalı JWT token") });
        }

        public string GetUserId(string token)
        {
            var principals = GetPrincipalFromToken(token);

            if (principals is not null)
            {
                return principals.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            }
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure> { new FluentValidation.Results.ValidationFailure("token", "Xətalı JWT token") });
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = _jwtOptions.ValidateIssuer,
                    ValidateAudience = _jwtOptions.ValidateAudience,
                    ValidateLifetime = false,
                    RequireExpirationTime = _jwtOptions.RequireExpirationTime,
                    ValidateIssuerSigningKey = _jwtOptions.ValidateIssuerSigningKey,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey)),
                };
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
