using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Programming_Tournament.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Programming_Tournament.Areas.Identity.Models;
using Programming_Tournament.Models.Domain.User;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Programming_Tournament
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(options => {
                var F = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                var L = F.Create("ModelBindingMessages", "Programming-Tournament");
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                    (x) => L["The value '{0}' is invalid."]);
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
                    (x) => L["The field {0} must be a number."]);
                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
                    (x) => L["A value for the '{0}' property was not provided.", x]);
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                    (x, y) => L["The value '{0}' is not valid for {1}.", x, y]);
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
                    () => L["A value is required."]);
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(
                    (x) => L["The supplied value is invalid for {0}.", x]);
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    (x) => L["Null value is invalid.", x]);
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddDataAnnotationsLocalization()
            .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ru") };
                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ru") };
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en")),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
