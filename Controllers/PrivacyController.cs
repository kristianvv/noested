using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Noested.Models;

namespace Noested.Controllers;

public class PrivacyController : Controller
{
   
    public IActionResult PrivacyPage()
    {
        return View();
    }

}

