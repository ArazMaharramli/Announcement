using System.Security.Claims;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserManager userManager)
    {
        UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        IsAuthenticated = UserId != null;
        if (IsAuthenticated)
        {
            var res = userManager.FindByIdAsync(UserId).Result;
            User = res;
        }
    }

    public string UserId { get; }

    public bool IsAuthenticated { get; }

    public UserDTO User { get; }

    public string AccessToken
    {
        get
        {
            return "";
        }
    }
}
