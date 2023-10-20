using Microsoft.AspNetCore.Mvc;
using Noested.Models;
using Noested.Models.DTOs;
using Noested.Data;
using Noested.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Noested.Controllers
{
    public class ServiceOrderController : Controller
    {
        private readonly ILogger<ServiceOrderController> _logger;
        private readonly IServiceOrderRepository _repository;
        private readonly ServiceOrderService _service;

        /* Midlertidig for controller-funksjoner fra slettede controllere */
        private readonly ServiceOrderDatabase _inMemoryDb;  // For DummyServiceOrdersController

        // Uses dependency injection to receive instance of db, Ilogger and a custom service
        public ServiceOrderController(IServiceOrderRepository repository, ILogger<ServiceOrderController> logger, ServiceOrderService service, /*midlertidig*/ ServiceOrderDatabase inMemoryDb) 
        {
            _repository = repository;
            _logger = logger;
            _service = service;

            _inMemoryDb = inMemoryDb; // Midlertidig DummyServiceOrdersController
        }

        //
        [HttpGet]
        public async Task<IActionResult> Index() // Class inherits from base Controller class to handle HTTP requests/responses
        {
            var allServiceOrders = await _repository.GetAllServiceOrdersAsync();
            if (allServiceOrders == null || !allServiceOrders.Any())
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "Null or no ServiceOrders in database"
                };
                return View("Error", errorViewModel);
            }
            return View(allServiceOrders);
        }
        //
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Passes existing customers to view's (Create.cshtml) dropdown list 
            var existingCustomers = await _repository.GetAllCustomersAsync(); // fetch
            ViewBag.ExistingCustomers = new SelectList(existingCustomers, "CustomerID", "FirstName"); // pass
            return View();
        }
        //
        [HttpPost]
        public async Task<IActionResult> Create(ServiceOrderModel newOrder, int? existingCustomerId)
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
                return View(newOrder);
            }
            else
            {
                _logger.LogInformation("Model state is valid.");
                if (existingCustomerId.HasValue) // using existing customer
                {
                    newOrder.Customer!.CustomerID = existingCustomerId.Value;
                }
                else
                {
                    if (newOrder.Customer != null && newOrder.Customer.CustomerID.HasValue) // null checks for CS8604 on adding
                    {
                        await _repository.AddCustomerAsync(newOrder.Customer); // adds new customer
                        newOrder.Customer!.CustomerID = newOrder.Customer.CustomerID.Value; // update foreign key
                    }
                    else
                    {
                        _logger.LogError("Customer information is incomplete or null.");
                        return View(newOrder);
                    }
                }

                await _repository.AddServiceOrderAsync(newOrder);
                _logger.LogInformation("New order added and redirecting to Index.");
                return RedirectToAction("Index");
            }
        }
        //
        public async Task<IActionResult> ViewOrder(int id)
        {
            var order = await _repository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            return View(order);
        }
        //
        [HttpPost]
        public async Task<IActionResult> SaveCompletedOrder(CompletedOrderDTO completedOrderDto)
        {
            _logger.LogInformation("Successfully called SaveCompletedOrder()");
            if (completedOrderDto == null || completedOrderDto.CompletedOrder == null)
            {
                return BadRequest("Invalid payload");
            }

            if (!ModelState.IsValid)
            {
                return View(completedOrderDto.CompletedOrder);
            }

            var existingOrder = await _repository.GetOrderByIdAsync(completedOrderDto.CompletedOrder.ServiceOrderID);
            if (existingOrder == null)
            {
                return NotFound("Requested Order Does Not Exist (Not Found)");
            }

            await _service.UpdateExistingOrderAsync(existingOrder, completedOrderDto.CompletedOrder, completedOrderDto.Form);

            return RedirectToAction("Index");
        }

        // REFAKTORERTE METODER fra DummyServiceOrdersController
        // Index
        public async Task<IActionResult> DummyIndex()
        {
            return _inMemoryDb.DummyServiceOrder != null ?
                          View(await Task.FromResult(_inMemoryDb.DummyServiceOrder)) :
                          Problem("Entity set 'ServiceOrderDatabase.DummyServiceOrder' is null.");
        }
        // Details
        public async Task<IActionResult> DummyDetails(int? id)
        {
            if (id == null || _inMemoryDb.DummyServiceOrder == null)
            {
                return NotFound();
            }

            var dummyServiceOrder = await Task.FromResult(_inMemoryDb.DummyServiceOrder.FirstOrDefault(m => m.ServiceOrderID == id));
            if (dummyServiceOrder == null)
            {
                return NotFound();
            }

            return View(dummyServiceOrder);
        }
        // Create
        public IActionResult DummyCreate()
        {
            return View();
        }
        // Create(DummyServiceOrder)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DummyCreate(DummyServiceOrder dummyServiceOrder)
        {
            if (ModelState.IsValid)
            {
                _inMemoryDb.DummyServiceOrder.Add(dummyServiceOrder);
                await Task.FromResult(0);  // Simulate async operation
                return RedirectToAction("DummyIndex");
            }
            return View(dummyServiceOrder);
        }
        // Edit(int? id)
        public async Task<IActionResult> DummyEdit(int? id)
        {
            if (id == null || _inMemoryDb.DummyServiceOrder == null)
            {
                return NotFound();
            }

            var dummyServiceOrder = await Task.FromResult(_inMemoryDb.DummyServiceOrder.FirstOrDefault(m => m.ServiceOrderID == id));
            if (dummyServiceOrder == null)
            {
                return NotFound();
            }
            return View(dummyServiceOrder);
        }
        // Edit(int id, DummyServiceOrder var)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DummyEdit(int id, DummyServiceOrder dummyServiceOrder)
        {
            if (id != dummyServiceOrder.ServiceOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update logic here
                await Task.FromResult(0);  // Simulate async operation
                return RedirectToAction("DummyIndex");
            }
            return View(dummyServiceOrder);
        }
        // Delete (int? id)
        public async Task<IActionResult> DummyDelete(int? id)
        {
            if (id == null || _inMemoryDb.DummyServiceOrder == null)
            {
                return NotFound();
            }

            var dummyServiceOrder = await Task.FromResult(_inMemoryDb.DummyServiceOrder.FirstOrDefault(m => m.ServiceOrderID == id));
            if (dummyServiceOrder == null)
            {
                return NotFound();
            }
            return View(dummyServiceOrder);
        }
        // DeleteConfirmed
        [HttpPost, ActionName("DummyDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DummyDeleteConfirmed(int id)
        {
            var dummyServiceOrder = await Task.FromResult(_inMemoryDb.DummyServiceOrder.FirstOrDefault(m => m.ServiceOrderID == id));
            if (dummyServiceOrder != null)
            {
                _inMemoryDb.DummyServiceOrder.Remove(dummyServiceOrder);
            }

            await Task.FromResult(0);  // Simulate async operation
            return RedirectToAction("DummyIndex");
        }
    }
}
