﻿


using Noested.Data;
using Noested.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Noested.Controllers;

public class LoginController : Controller
    {
        private readonly ServiceOrderDatabase _context;

        public LoginController(ServiceOrderDatabase context)
        {
            _context = context;
        }

    // Landing side
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

    [HttpPost]
        public IActionResult Login(int employeeNumber, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmployeeNumber == employeeNumber && u.Password == password);

        if (user != null)
        {
            switch (user.Role)
            {
                case UserRole.Service:
                    return RedirectToAction("ServicePersonellPage", "ServicePersonell");
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



