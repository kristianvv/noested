


using Noested.Data;
using Noested.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;

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
    [ValidateAntiForgeryToken]
    public IActionResult RegisterUser(RegistreringViewModel model)
    {
        // Convert role string to enum
        Enum.TryParse(model.Role, out UserRole userRole);

        var user = new User
        {
            EmployeeNumber = model.EmployeeNumber,
            Password = HashPassword(model.Password),
            Role = userRole
        };

        _context.Users.Add(user);

        return RedirectToAction("Index", "Login"); // Redirect to login page after successful registration
    }

    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
        public IActionResult Login(int employeeNumber, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmployeeNumber == employeeNumber && u.Password == HashPassword(password));

            if (user != null)
            {
                switch (user.Role)
                {
                    case UserRole.Service:
                        return RedirectToAction("Index", "ServiceOrder");//IKKEEEE ORDERSSSSSSSSSS
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



