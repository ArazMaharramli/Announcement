using System.Security.Claims;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserManager userManager)
    {
        UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        IsAuthenticated = UserId != null;

        if (IsAuthenticated && User is null)
        {
            var res = userManager.FindByIdAsync(UserId).Result;
            User = res;
        }
    }

    public string UserId { get; }

    public bool IsAuthenticated { get; }

    public UserDTO User { get; }
}