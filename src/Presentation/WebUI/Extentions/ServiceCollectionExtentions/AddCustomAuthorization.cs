using System;
using Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI.Extentions.ServiceCollectionExtentions;

public static class AddCustomAuthorization
{
    public static IServiceCollection AddCustomRoleAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            foreach (var claimgroup in SystemClaims.GetSystemClaims())
            {
                foreach (var claim in claimgroup.Claims)
                {
                    options.AddPolicy(claim.Value,
                        policy => policy.RequireClaim(claim.Value));
                }
            }
        });

        return services;
    }
}
