using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Noested.Data;
using Noested.Services;
using Noested.Utilities;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = builder.Configuration.GetConnectionString("NoestedContext");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>(); // Daniel repository pattern for in-mem DB
builder.Services.AddSingleton<ServiceOrderDatabase>(); // Daniel (AppDbContext (EFCore) og NoestedContext (SQL) test variabler flyttet hit)
builder.Services.AddScoped<ServiceOrderService>(); // Daniel
builder.Services.AddScoped<ChecklistService>(); // Daniel
builder.Services.AddScoped<CustomerService>(); // Daniel
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("Mechanic", policy => policy.RequireRole("Mechanic"));
    options.AddPolicy("Service", policy => policy.RequireRole("Service"));
});

//Login og logout
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // Set the login path
        options.LogoutPath = "/logout"; // Set the logout path
        options.AccessDeniedPath = "/Login/AccessDenied"; // Set the access denied path
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

app.UseAuthentication();
app.UseAuthorization();

// Login
app.MapControllerRoute(
    name: "login",
    pattern: "/login",
    defaults: new { controller = "Login", action = "Index" }
);

// Register
app.MapControllerRoute(
    name: "register",
    pattern: "/register",
    defaults: new { controller = "Register", action = "RegisterUser" }
);

app.MapRazorPages();

// Default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

// Access denied
app.MapControllerRoute(
    name: "accessdenied",
    pattern: "/Account/AccessDenied",
    defaults: new { controller = "Login", action = "AccessDenied" }
);

// Headers beskyttelse

WebHost.CreateDefaultBuilder(args)
    .ConfigureKestrel(c => c.AddServerHeader = false)
    .UseStartup<Noested.Startup>()
    .Build();

app.Run();
