using System.Net.NetworkInformation;
using System.Linq;
using System.Globalization;
using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using WebUI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System.Collections.Generic;
using WebUI.Models.ConfigModels;
using Localization.SqlLocalizer.DbStringLocalizer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebUI.Middlewares;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        List<LanguageModel> supportedLanguages = new List<LanguageModel>();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddPersistence(Configuration);
            services.AddApplication();

            services.AddAuthentication();

            services.AddDbContext<LocalizationModelContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("LocalizationDb"),
                   b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
               ),
               ServiceLifetime.Singleton,
               ServiceLifetime.Singleton
           );
            var useTypeFullNames = true;
            var useOnlyPropertyNames = false;
            var returnOnlyKeyIfNotFound = false;
            var createNewRecordWhenLocalisedStringDoesNotExist = true;

            // Requires that LocalizationModelContext is defined
            // _createNewRecordWhenLocalisedStringDoesNotExist read from the dev env. 
            services.AddSqlLocalization(options => options.UseSettings(
                useTypeFullNames,
                useOnlyPropertyNames,
                returnOnlyKeyIfNotFound,
                createNewRecordWhenLocalisedStringDoesNotExist));
            // services.AddSqlLocalization(options => options.ReturnOnlyKeyIfNotFound = true);
            // services.AddLocalization(options => options.ResourcesPath = "Resources");

            // services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options => GetRequestLocalizationOptions());


            Configuration.Bind("SupportedLanguages", supportedLanguages);

            services.AddSingleton<SupportedLanguages>(new SupportedLanguages { Languages = supportedLanguages });

            services.AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<IDbContext>();

            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ICurrentLanguageService, CurrentLanguageService>();

            services.AddHealthChecks()
               .AddDbContextCheck<ApplicationDbContext>()
               .AddDbContextCheck<MainDbContext>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });
            services.AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<GzipCompressionProvider>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCustomExceptionHandler();
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseHealthChecks("/healthchecks");

            app.UseRequestLocalization(GetRequestLocalizationOptions());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaswithlang",
                    pattern: "{lang}/{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private RequestLocalizationOptions GetRequestLocalizationOptions()
        {
            var supportedCultures = supportedLanguages.Select(x => new CultureInfo(x.Culture)).ToArray();
            var options = new RequestLocalizationOptions()
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                DefaultRequestCulture = new RequestCulture(supportedCultures[0])

            };

            options.ApplyCurrentCultureToResponseHeaders = true;
            // options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider
            {
                RouteDataStringKey = "lang",
                Options = options,
            });
            options.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());

            return options;

        }
    }
}
