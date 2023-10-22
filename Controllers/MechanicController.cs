using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Noested.Models;
using Noested.Services;
using System.Text;
using System.Threading.Tasks;

namespace Noested.Controllers
{
    public class MechanicController : Controller
    {
        private readonly ILogger<MechanicController> _logger;
        private readonly ServiceOrderService _serviceOrderService;

        public MechanicController(ILogger<MechanicController> logger, ServiceOrderService serviceOrderService)
        {
            _logger = logger;
            _serviceOrderService = serviceOrderService;
        }

        public async Task<IActionResult> MechanicPage()
        {
            _logger.LogInformation("MechanicPage(): Called");
            try
            {
                var allServiceOrders = await _serviceOrderService.FetchAllServiceOrdersAsync();
                _logger.LogInformation("MechanicPage(): Retrieved all service orders");
                return View(allServiceOrders);
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

        public async Task<IActionResult> ViewOrder(int id)
        {
            _logger.LogInformation("ViewOrder(): Called");
            try
            {
                var order = await _serviceOrderService.FetchServiceOrderByIdAsync(id);
                _logger.LogInformation("ViewOrder(): Retrieved order");
                return View("~/Views/ServiceOrder/ViewOrder.cshtml", order);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveCompletedOrder(ServiceOrderModel completedOrder, IFormCollection form)
        {
            Request.EnableBuffering();

            // Read the request body
            var buffer = new byte[Convert.ToInt32(Request.ContentLength)];
            await Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);

            // Log the request body
            _logger.LogInformation($"Request Body: {requestBody}");

            // Reset the request body stream position
            Request.Body.Position = 0;


            _logger.LogInformation("SaveCompletedOrder(): Called");

            var validationResult = await _serviceOrderService.UpdateCompletedOrderAsync(completedOrder, form);

            if (validationResult)
            {
                _logger.LogInformation("SaveCompletedOrder(): Success");
                return RedirectToAction("MechanicPage");
            }
            else
            {
                _logger.LogError("SaveCompletedOrder(): Failed");
                return View("Error", new ErrorViewModel { RequestId = "Failed to save completed order" });
            }
        }
    }
}
