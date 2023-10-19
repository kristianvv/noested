using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Noested.Data;

using Noested.Services;
using Noested.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Register AppDbContext with in-memory database for testing
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));

// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = builder.Configuration.GetConnectionString("NoestedContext");

// Register NoestedContext with SQL Server database
builder.Services.AddDbContext<NoestedContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ServiceOrderDatabase>();
builder.Services.AddScoped<ServiceOrderService>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
builder.Services.AddScoped<ChecklistService>();


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

/* Retrieve the ServiceOrderDatabase instance from the service provider */
using (var serviceScope = app.Services.CreateScope())
{
    // Retrieve instance of database
    var serviceOrderDatabase = serviceScope.ServiceProvider.GetRequiredService<ServiceOrderDatabase>();

    // Seed the database with hardcoded service orders
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
