using Microsoft.AspNetCore.Mvc;
using Noested.Models;
using System.Diagnostics;

namespace Noested.Controllers
{
    
    
    public class MechanicController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MechanicPage()
        {
            return View();
        }
    }
  
}
 