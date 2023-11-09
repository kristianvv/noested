using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
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

// Headers beskyttelse

WebHost.CreateDefaultBuilder(args)
    .ConfigureKestrel(c => c.AddServerHeader = false)
    .UseStartup<Noested.Startup>()
    .Build();

app.Run();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
