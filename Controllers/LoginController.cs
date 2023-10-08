
namespace Innlogging.Controllers;

using Noested.Data;
using Innlogging.Models;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        public LoginController(AppDbContext context)
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
        _context.SaveChanges();

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
                        return RedirectToAction("ServicePage", "Home");
                    case UserRole.Mechanic:
                        return RedirectToAction("MechanicPage", "Home");
                    case UserRole.Administrator:
                        return RedirectToAction("AdminPage", "Home");
                }
            }
            ViewBag.ErrorMessage = "Feil ansattnummer eller passord";
            return View("Index");
        }
    }



