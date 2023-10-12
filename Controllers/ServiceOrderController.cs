using Microsoft.AspNetCore.Mvc;
using Noested.Models;
using Noested.Models.DTOs;
using Noested.Data;
using Noested.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace Noested.Controllers
{
    public class ServiceOrderController : Controller
    {
        /* dependency injection variables */
        private readonly ILogger<ServiceOrderController> _logger;
        private readonly IServiceOrderRepository _repository;
        private readonly ServiceOrderService _service;

        public ServiceOrderController(IServiceOrderRepository repository, ILogger<ServiceOrderController> logger, ServiceOrderService service) // interacts with repository through interface
        {
            _repository = repository;
            _logger = logger;
            _service = service;
        }

        /* Returns: All Service Orders from repository */
        [HttpGet]
        public IActionResult Index()
        {
            var allServiceOrders = _repository.GetAllServiceOrders();
            if (allServiceOrders == null || !allServiceOrders.Any())
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "Null or no ServiceOrders in database"
                };
                return View("Error", errorViewModel); // Error.cshtml in Views folder.
            }
            return View(allServiceOrders);

        }

        /* Summary: Service Personell Viewscreen for Creating a New Service Order.
         * Returns: View > ServiceOrder > Create.cshtml */
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /* Summary: Submits a New Service Order to the database (Service Personell)
         * Param name="newOrder" – Accepts formdata (Create.cshtml) based on model.
         * Returns: To the Mechanics view (if valid) or back to same view (if validation errors)  */
        [HttpPost]
        public IActionResult Create(ServiceOrderModel newOrder)
        {
            _logger.LogInformation("Successfully called Create Method"); // Invoking Create Method
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)  // Loop through ModelState, Log validation errors.
                {
                    if (modelState.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Error in field {modelState.Key}: {modelState.Value.Errors[0].ErrorMessage}");
                    }
                }
                return View(newOrder);
            }

            // Log valid model state
            _logger.LogInformation("Model state is valid.");

            // Add new Service Order and log the action
            _repository.AddServiceOrder(newOrder);
            _logger.LogInformation("New order added and redirecting to Index.");

            // Redirect to the Mechanics view
            return RedirectToAction("Index");
        }

        /* Summary: Opens a Service Order from Mechanics' Viewscreen.
         * Param name="id" – Identifies Service Order based on OrderNumber.
         * Returns: Single Service Order from database to View (if found) or nullcheck "order not found" */
        public IActionResult ViewOrder(int id)
        {
            var order = _repository.GetOrderById(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            return View(order);
        }

        /* Summary: Saves Completed Service Order and Updates its Status.
         * Param=name"openedOrder" – Service Order that has been Opened for Completion
         * Param=name"form" – Form Collection Containing Checklist Items (serviceorder.js)
         * Returns: The Action Result.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCompletedOrder(CompletedOrderDto completedOrderDto)
        {
            _logger.LogInformation("Successfully called SaveCompletedOrder()");
            _logger.LogInformation($"Received DTO: {JsonSerializer.Serialize(completedOrderDto)}");


            // Validate incoming data
            if (completedOrderDto == null || completedOrderDto.CompletedOrder == null)
            {
                _logger.LogError("Either completedOrderDto or completedOrderDto.CompletedOrder is null.");
                return BadRequest("Invalid Data");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is not valid. ModelState: {ModelState}", ModelState);
                return Json(new { status = "error", message = "ModelState is not valid." });
            }

            // Get existing Service Order for update
            var existingOrder = _repository.GetOrderById(completedOrderDto.CompletedOrder.OrderNumber);
            if (existingOrder == null)
            {
                return Json(new { status = "error", message = "Requested Order Does Not Exist (Not Found)" });
            }

            // Update existing Service Order
            _service.UpdateExistingOrder(existingOrder, completedOrderDto.CompletedOrder, completedOrderDto.Form);  // Root>Services>ServiceOrderServices.cs
            return Json(new { status = "success", message = "The order has been successfully saved." });

            // Redirect to Mechanics view
            // return RedirectToAction("Index");
        }
    }
}

