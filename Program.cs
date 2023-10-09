using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Noested.Data;


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

var app = builder.Build();

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
