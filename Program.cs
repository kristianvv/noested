using Microsoft.AspNetCore.Authentication.Cookies;
using Noested.Data;
using Noested.Services;
using Noested.Utilities;

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "login",
    pattern: "/login",
    defaults: new { controller = "Login", action = "Index" }
);

app.MapControllerRoute(
    name: "register",
    pattern: "/register",
    defaults: new { controller = "Register", action = "RegisterUser" }
);

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
