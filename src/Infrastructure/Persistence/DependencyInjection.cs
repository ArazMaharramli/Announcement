using System;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("MainDb"),
                    x =>
                    {
                        x.UseNetTopologySuite();
                        x.MigrationsAssembly(typeof(MainDbContext).Assembly.FullName);
                    }).LogTo(Console.WriteLine),
                    ServiceLifetime.Scoped);

            services.AddScoped<IDbContext>(provider => provider.GetService<MainDbContext>());

            return services;
        }
    }
}
