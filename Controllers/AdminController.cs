using Microsoft.AspNetCore.Mvc;

namespace Noested.Controllers
{
   
    public class AdminController : Controller
    {
       
        public IActionResult AdminPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewUser()
        {
            return RedirectToAction("RegisterUser", "Account");
        }


    }
}
