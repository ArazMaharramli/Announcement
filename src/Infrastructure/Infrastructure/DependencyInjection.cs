using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Common;
using Infrastructure.Common;
using Infrastructure.Identity;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserManager, UserManagerService>();

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDateTimeService, MachineDateTime>();


            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));
            services.AddTransient<IEmailService, EmailService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityDB")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.Configure<StaticUrls>(configuration.GetSection("StaticUrls"));


            return services;

        }
    }
}
