
using Microsoft.AspNetCore.Mvc;
using Noested.Data;
using Noested.Models;


namespace Noested.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ServiceOrderDatabase _context;
        public RegisterController(ServiceOrderDatabase context)
        {
            _context = context;
        }

        public IActionResult RegisterUser()
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
                Email = model.Email,
                Password = model.Password,
                Role = userRole
            };

            _context.Users.Add(user);

            return RedirectToAction("Index", "Login"); // Redirect to home page after successful registration
        }

    }

}
