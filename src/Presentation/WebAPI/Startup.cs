using System;
using System.Reflection;
using System.Text;
using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using WebAPI.Middlewares;
using WebAPI.Models;
using WebAPI.Services;
using Application.Common.Models.ConfigModels;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddPersistence(Configuration);
            services.AddApplication();

            var jwtOptions = new JwtOptions();
            Configuration.Bind("Jwt", jwtOptions);

            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IAuthTokenProvider, JwtTokenProvider>();
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = jwtOptions.ValidateIssuer,
                        ValidateAudience = jwtOptions.ValidateAudience,
                        ValidateLifetime = jwtOptions.ValidateLifetime,
                        RequireExpirationTime = jwtOptions.RequireExpirationTime,
                        ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecurityKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>()
            .AddDbContextCheck<MainDbContext>();

            TenantInfo tenantInfo = new TenantInfo();
            Configuration.Bind("TenantInfo", tenantInfo);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = tenantInfo.Name,
                    Version = "V1",
                    Contact = new OpenApiContact { Email = tenantInfo.Contact.Email, Name = tenantInfo.Contact.Name },
                    Description = tenantInfo.ShortDescription
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT is required to use this api",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                     }, new string[] { }
                }
              });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCustomExceptionHandler();

            app.UseHealthChecks("/healthchecks");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                   name: "Areas",
                   pattern: "/v{version}/{area:exists}/{controller}/{action}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/v{version}/{controller}/{action}/{id?}");
            });
        }
    }
}
