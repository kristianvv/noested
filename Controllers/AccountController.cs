using System;
using Microsoft.AspNetCore.Mvc;
using innlogging.Models.Data;
using innlogging.Models;
using Microsoft.EntityFrameworkCore;


namespace innlogging.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // Convert role string to enum
            Enum.TryParse(model.Role, out UserRole userRole);

            var user = new User
            {
                EmployeeNumber = model.EmployeeNumber,
                Email = model.Email,
                Password = model.Password,
                Role = userRole
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home"); // Redirect to home page after successful registration
        }

    }

}
