using Microsoft.AspNetCore.Mvc;

namespace Noested.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
