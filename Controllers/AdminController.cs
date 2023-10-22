using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Noested.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
       
        public IActionResult AdminPage()
        {
            return View();
        }

        public IActionResult CreateNewUser()
        {
            return RedirectToAction("RegisterUser", "Account");
        }


    }
}
