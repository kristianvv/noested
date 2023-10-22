


using Noested.Data;
using Noested.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Noested.Controllers;

public class LoginController : Controller
    {
        private readonly ServiceOrderDatabase _context;

        public LoginController(ServiceOrderDatabase context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    [HttpPost]
    public IActionResult RegisterUser(RegistreringViewModel model)
    {
        // Convert role string to enum
        Enum.TryParse(model.Role, out UserRole userRole);

        var user = new User
        {
            EmployeeNumber = model.EmployeeNumber,
            Password = model.Password,
            Role = userRole
        };

        _context.Users.Add(user);

        return RedirectToAction("Index", "Login"); // Redirect to login page after successful registration
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        ViewData["ErrorMessage"] = "Access Denied";
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(int employeeNumber, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.EmployeeNumber == employeeNumber && u.Password == password);

        if (user != null)
        {
            // Create claims
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.EmployeeNumber.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
      
        };

            // Create identity
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create principal
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            switch (user.Role)
            {
                case UserRole.Service:
                    return RedirectToAction("Index", "ServiceOrder");
                case UserRole.Mechanic:
                    return RedirectToAction("MechanicPage", "Mechanic");
                case UserRole.Administrator:
                    return RedirectToAction("AdminPage", "Admin");
            }
        }

        ViewBag.ErrorMessage = "Feil ansattnummer eller passord";
        return View("Index");
    }


    public IActionResult Logout()
    {
        // Perform logout logic
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Login"); // Redirect to login page
    }
}



