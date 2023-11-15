using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Data.Repositories;
using Noested.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString,
    new MySqlServerVersion(new Version(10, 5, 11))));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>(); // Daniel
builder.Services.AddScoped<ServiceOrderService>(); // Daniel
builder.Services.AddScoped<ChecklistService>(); // Daniel
builder.Services.AddScoped<CustomerService>(); // Daniel

var app = builder.Build();


// Database seeding
using (var serviceScope = app.Services.CreateScope())
{
    var serviceOrderDatabase = serviceScope.ServiceProvider.GetRequiredService<ServiceOrderRepository>();
    //For testing purposes er typen til GetRequiredService endret til ServiceOrderRepository

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
        "style-src 'self' 'unsafe-inline' https://stackpath.bootstrapcdn.com; " +
        "script-src 'self'; " +
        "frame-src 'self'; " +
        "connect-src 'self';");
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapFallbackToAreaPage("/Account/Login", "Identity");

app.Run();