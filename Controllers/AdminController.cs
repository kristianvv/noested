using Microsoft.AspNetCore.Mvc;

namespace Noested.Controllers
{
    public class AdminController : Controller
    {
       
        public IActionResult AdminPage()
        {
            return View();
        }



    }
}
