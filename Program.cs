using Microsoft.AspNetCore.Authentication.Cookies;
using Noested.Data;
using Noested.Services;
using Noested.Utilities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Antiforgery;
using Noested;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = builder.Configuration.GetConnectionString("NoestedContext");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ServiceOrderDatabase>(); // Daniel (AppDbContext (EFCore) og NoestedContext (SQL) test variabler flyttet hit)
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>(); // Daniel repository pattern for in-mem DB
builder.Services.AddScoped<ServiceOrderService>(); // Daniel
builder.Services.AddScoped<ChecklistService>(); // Daniel
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN"; // Velg et passende navn for headeren?
});

//Login og logout
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // Set the login path
        options.LogoutPath = "/logout"; // Set the logout path
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set the cookie expiration time
        options.SlidingExpiration = true;
    });


var app = builder.Build();

/* Get the Repo instance (Repository Pattern) from the service provider for Daniel DbSeeder... */
using (var serviceScope = app.Services.CreateScope())
{
    // ...with this line...
    var serviceOrderDatabase = serviceScope.ServiceProvider.GetRequiredService<IServiceOrderRepository>();

    // ... and seed DB with hardcoded service orders
    DatabaseSeeder.SeedServiceOrders(serviceOrderDatabase);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.Use(next => context =>
{
    var antiforgery = context.RequestServices.GetService<IAntiforgery>();
    // Send Antiforgery cookie token as a response header
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("X-CSRF-TOKEN", tokens.RequestToken, new CookieOptions
    {
        HttpOnly = false, // Make the cookie accessible via JavaScript
        Secure = true, // Send only over HTTPS if possible
        SameSite = SameSiteMode.None // Allow cross-origin cookies
    });
    return next(context);
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "login",
    pattern: "/login",
    defaults: new { controller = "Login", action = "Index" });


/*app.MapControllerRoute(
name: "mechanicLogin",
pattern: "/login/mechanic",
defaults: new { controller = "Mechanic", action = "Login" });

    app.MapControllerRoute(
    name: "serviceLogin",
    pattern: "/serviceorder/login",
    defaults: new { controller = "ServiceOrders", action = "Login" }


);*/

app.MapControllerRoute(
    name: "register",
    pattern: "/register",
    defaults: new { controller = "Register", action = "RegisterUser" }
);

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

WebHost.CreateDefaultBuilder(args)
    .ConfigureKestrel(c => c.AddServerHeader = false)
    .UseStartup<Startup>()
    .Build();

app.Run();
