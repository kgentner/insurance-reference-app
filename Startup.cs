using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using Reggie.Data;
using Reggie.Models;
using Reggie.Services;
using Reggie.ViewModels;


namespace Reggie
{
    public class Startup
    {
        private IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IISOptions>(options => options.ForwardWindowsAuthentication = true);

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Hourly", new CacheProfile()
                {
                    Duration = 60 * 60 // 1 hour
                });
                options.CacheProfiles.Add("HalfDaily", new CacheProfile()
                {
                    Duration = 60 * 60 * 12 // 12 hours
                });
                options.CacheProfiles.Add("Daily", new CacheProfile()
                {
                    Duration = 60 * 60 * 24 // 1 day
                });
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Add Response caching
            services.AddResponseCaching();


            // Add below to ensure https for PROD
            //
            // services.AddMvc(config =>
            // {
            // if (_env.IsProduction())
            // {
            //     config.Filters.Add(new RequireHttpsAttribute());
            // }    
            // });

            // From https://andrewlock.net/introduction-to-authorisation-in-asp-net-core/
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "CanDoAnything",
                    policy => policy. RequireRole("Admin"));
            });


            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Cookie settings
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Account/AccessDenied";
                // options.Cookies.ApplicationCookie.CookieName = "ReggieCookie";
                // options.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                // options.Cookies.ApplicationCookie.AuthenticationScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;

                // User settings
                options.User.RequireUniqueEmail = true;


            });

            services.AddLogging();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            

            app.UseResponseCaching();

            //Redirect all HTTP requests to HTTPS
            //
            // var options = new RewriteOptions()
            //     .AddRedirectToHttps();
            // app.UseRewriter(options);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
             }

            //Caching method for static files)
            
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 7; //Weekly
                    context.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseIdentity();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "insurance-index",
                    template: "insurance",
                    defaults: new { controller = "insurance", action = "index" }
                );

                routes.MapRoute(
                    name: "home-index",
                    template: "",
                    defaults: new { controller = "home", action = "index" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

                //For Seeding Database. Set password with the Secret Manager tool. $ dotnet user-secrets set SeedUserPW <pw>

                var testUserNm = Configuration["SeedUserNM"];
                var testUserPw = Configuration["SeedUserPW"];

                if (String.IsNullOrEmpty(testUserPw))
                {
                    throw new System.Exception("Use secrets manager to set SeedUserNM or SeedUserPW \n" +
                                            "dotnet user-secrets set SeedUserPW <pw>");
                }

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                                .Database.EnsureDeleted();
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                                .Database.EnsureCreated();
                    }
                SeedData.Initialize(app.ApplicationServices, testUserPw, testUserNm).Wait();
        }
    }
}
