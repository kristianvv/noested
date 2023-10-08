
using Microsoft.AspNetCore.Mvc;
using Noested.Data;
using Innlogging.Models;


namespace Innlogging.Controllers
{
    public class RegisterController : Controller
    {
        private readonly AppDbContext _context;
        public RegisterController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult RegisterUser()
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
                Email = model.Email,
                Password = model.Password,
                Role = userRole
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Login"); // Redirect to home page after successful registration
        }

    }

}
