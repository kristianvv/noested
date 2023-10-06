using System;
using innlogging.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace innlogging.Models
{
	public class Startup
	{
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services here
            services.AddControllersWithViews();
           // services.AddRazorPages();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                // Other identity options...
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // Other services...
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "/login",
                    defaults: new { controller = "Login", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "register",
                    pattern: "/register",
                    defaults: new { controller = "Login", action = "Register" }
                );
            });

            // Other middleware components can go here...
        }


        // Other middleware components can go here...

    }
}


