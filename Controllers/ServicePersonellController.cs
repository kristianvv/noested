using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noested.Models;
using Noested.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noested.Controllers
{
    public class ServicePersonellController : Controller
    {
        private readonly ILogger<MechanicController> _logger;
        private readonly ServiceOrderService _serviceOrderService;

        // Bruker ikke repository, men service
        public ServicePersonellController(ILogger<MechanicController> logger, ServiceOrderService serviceOrderService)
        {
            _logger = logger;
            _serviceOrderService = serviceOrderService;
        }

        // Omdir fra innlogging
        public IActionResult ServicePersonellPage()
        {
            _logger.LogInformation("Successfully called ServicePersonellPage()");
            return View();
        }

        // Se alle eksisterende ordre // må lages
        [HttpGet]
        public async Task<IActionResult> SeAlleOrdre()
        {
            _logger.LogInformation("Successfully called SeAlleOrdre()");
            try
            {
                var allServiceOrders = await _serviceOrderService.FetchAllServiceOrdersAsync(); // bruker service
                _logger.LogInformation("Successfully retrieved serviceorders via ServiceOrderService.cs");
                return View("~/Views/ServiceOrder/Index.cshtml", allServiceOrders);
            }
            catch (InvalidOperationException ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ex.Message
                };
                return View("Error", errorViewModel);
            }
        }

        // Omdir opprette serviceordre – må hente eksisterende kunder fra db for dropdown meny.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Successfully called Create() in ServicePersonellController");
            try
            {
                var existingCustomers = await _serviceOrderService.FetchAllCustomersAsync(); // bruke service
                _logger.LogInformation("Successfully fetched all customers from serviceOrderService");
                ViewBag.ExistingCustomers = new SelectList(existingCustomers, "CustomerID", "FirstName");
                _logger.LogInformation("Successfully populated data from service into ViewBag, sending to View");
                return View("~/Views/ServiceOrder/Create.cshtml"); // Omdir Create.cshtml i ServiceOrdre viewet
            }
            catch (InvalidOperationException ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = ex.Message
                };
                return View("Error", errorViewModel);
            }
        }

        // Lage ny Serviceordre med eller uten eksisterende kunde
        [HttpPost]
        public async Task<IActionResult> Create(ServiceOrderModel newOrder, int? existingCustomerId)
        {
            _logger.LogInformation("Successfully called Create Method");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is not valid");
                foreach (var modelState in ModelState)
                {
                    if (modelState.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Error in field {modelState.Key}: {modelState.Value.Errors[0].ErrorMessage}");
                    }
                }
                return View(newOrder);
            }
            else
            {
                _logger.LogInformation("Model state is valid.");
                bool isSuccessful = await _serviceOrderService.CreateNewServiceOrderAsync(newOrder, existingCustomerId); // bruke service
                if (isSuccessful)
                {
                    _logger.LogInformation("New order added and redirecting to Index.");
                    return RedirectToAction("ServicePersonellPage");
                }
                else
                {
                    _logger.LogError("Customer information is incomplete or null.");
                    return View(newOrder);
                }
            }
        }


        //


    }
}

