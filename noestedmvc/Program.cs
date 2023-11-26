using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Data.Repositories;
using Noested.Repositories;
using Noested.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
    new MySqlServerVersion(new Version(10, 5, 11))));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>(); // Daniel
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ServiceOrderService>(); // Daniel
builder.Services.AddScoped<ChecklistService>(); // Daniel
builder.Services.AddScoped<CustomerService>(); // Daniel

var app = builder.Build();


// Database seeding
using (var serviceScope = app.Services.CreateScope())
{
    var serviceOrderDatabase = serviceScope.ServiceProvider.GetRequiredService<IServiceOrderRepository>();
    await DatabaseSeeder.SeedServiceOrders(serviceOrderDatabase);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Tilpasset Middleware for Sikkerhetsheadere
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Xss-Protection", "1");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add(
        "Content-Security-Policy",
        "default-src 'self'; " +
        "img-src 'self'; " +
        "font-src 'self'; " +
        "style-src 'self' 'unsafe-inline' https://stackpath.bootstrapcdn.com https://cdn.jsdelivr.net; " +
        "script-src 'self' https://code.jquery.com https://cdn.jsdelivr.net https://stackpath.bootstrapcdn.com; " +
        "frame-src 'self'; " +
        "connect-src 'self' wss://localhost:44301;");
    await next();
});



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToAreaPage("/Account/Login", "Identity");

// Database seeding av roller

using (var serviceScope = app.Services.CreateScope())
    {
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "User"};
    foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                {       
            await roleManager.CreateAsync(new IdentityRole(role));
                }
        }
    }

// Database seeding av adminbruker

using (var serviceScope = app.Services.CreateScope())
{
    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@admin.com";
    string password = "Admin123!";


    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }

 }

    app.Run();
