using System;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace WebUI.Services;

public class AdditionalUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AdditionalUserClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOptions<IdentityOptions> optionsAccessor)
    : base(userManager, roleManager, optionsAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }



    public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = (ClaimsIdentity)principal.Identity;

        var claims = await _userManager.GetClaimsAsync(user);
        if (claims is not null && claims.Count > 0)
        {
            var claimsToRemove = identity.Claims.Where(x => x.Type == ClaimTypes.Role || x.Type == ClaimTypes.UserData).ToList();
            foreach (var item in claimsToRemove)
            {
                identity.RemoveClaim(item);
            }

            identity.AddClaims(claims);
        }
        else
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleclaims = roles.SelectMany(x =>
            {
                var role = _roleManager.FindByNameAsync(x).Result;
                return _roleManager.GetClaimsAsync(role).Result;
            }).ToList();

            identity.AddClaims(claims);
        }

        return principal;
    }
}
