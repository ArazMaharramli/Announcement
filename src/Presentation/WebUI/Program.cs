﻿using Infrastructure.Identity;
using Localization.SqlLocalizer.DbStringLocalizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var identityDb = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                identityDb.Database.Migrate();

                var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
                db.Database.Migrate();

                var localizationDb = scope.ServiceProvider.GetRequiredService<LocalizationModelContext>();
                localizationDb.Database.Migrate();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                });
    }
}
