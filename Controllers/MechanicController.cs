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
            try
            {
                var allServiceOrders = await _serviceOrderService.FetchAllServiceOrdersAsync();
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
