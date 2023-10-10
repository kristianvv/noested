using Microsoft.AspNetCore.Mvc;
using Noested.Models;
using System.Diagnostics;

namespace Noested.Controllers
{
    public class MechanicController : Controller
    {
        public IActionResult MechanicPage()
        {
            return View();
        }
    }
  
}
 