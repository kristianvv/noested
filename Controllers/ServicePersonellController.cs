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
        private readonly CustomerService _customerService;

        // Bruker ikke repository, men service
        public ServicePersonellController(ILogger<MechanicController> logger, ServiceOrderService serviceOrderService, CustomerService customerService)
        {
            _logger = logger;
            _serviceOrderService = serviceOrderService;
            _customerService = customerService;
        }

        // Omdir fra innlogging
        public IActionResult ServicePersonellPage()
        {
            return View();
        }

        // Side for å opprette serviceordre – må hente eksisterende kunder for nedtrekksmenyen der.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var existingCustomers = await _customerService.FetchAllCustomersAsync(); // bruke service
                ViewBag.ExistingCustomers = new SelectList(existingCustomers, "CustomerID", "FirstName");
                _logger.LogInformation("Successfully populated data from service into ViewBag, sending to View");
                return View("~/Views/ServiceOrder/Create.cshtml"); // Omdir-lenke nødvendig pga mappestruktur
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
        public async Task<IActionResult> Create(ServiceOrderModel newOrder)
        {
            _logger.LogInformation("Successfully called Create Method");
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    if (modelState.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Error in field {modelState.Key}: {modelState.Value.Errors[0].ErrorMessage}");
                    }
                }
                return View("~/Views/ServiceOrder/Create.cshtml", newOrder); 
            }
            else
            { // (Model valid)
                bool isSuccessful = await _serviceOrderService.CreateNewServiceOrderAsync(newOrder); // bruke service
                if (isSuccessful)
                {
                    return RedirectToAction("ServicePersonellPage");
                }
                else
                {
                    _logger.LogError("Customer information is incomplete or null.");
                    return View("~/Views/ServiceOrder/Create.cshtml", newOrder); // Visual display for user what's wrong?
                }
            }
        }


        /* Registrere ny kunde uavhengig av serviceordre
        [HttpGet]
        public async Task<IActionResult> RegisterCustomer()
        {
            return View();
        }
        */

        // Se alle eksisterende ordre // må lages nytt view
        [HttpGet]
        public async Task<IActionResult> SeAlleOrdre()
        {
            try
            {
                var allServiceOrders = await _serviceOrderService.FetchAllServiceOrdersAsync(); // bruker service
                _logger.LogInformation("Successfully retrieved serviceorders via ServiceOrderService.cs");
                return View("~/Views/ServiceOrder/Index.cshtml", allServiceOrders); // midlertidig view
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


    }
}

