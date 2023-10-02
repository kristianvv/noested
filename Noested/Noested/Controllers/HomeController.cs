using Noested.DataAccess;
using Noested.Models;
using Noested.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Noested.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Index method called");

            var model = new RazorViewModel
            {
                Content = "Noested sin Content",
                AdditionalData = "Noested sin AdditionalData"
            };
            return View("Index", model);
        }
    }
}