using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Noested.Data;
using Noested.Services;
using Noested.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NoestedContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NoestedContext") ?? throw new InvalidOperationException("Connection string 'NoestedContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ServiceOrderDatabase>();
builder.Services.AddScoped<ServiceOrderService>();
builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
builder.Services.AddScoped<ChecklistService>();

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
