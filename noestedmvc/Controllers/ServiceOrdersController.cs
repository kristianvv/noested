using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Models;
using Noested.Services;

namespace Noested.Controllers
{
    [Authorize (Roles = "Admin, User")]
    public class ServiceOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceOrdersController> _logger;
        private readonly CustomerService _customerService;
        private readonly ServiceOrderService _serviceOrderService;
        private readonly ChecklistService _checklistService;

        public ServiceOrdersController(ApplicationDbContext context, ILogger<ServiceOrdersController> logger, CustomerService customerService, ServiceOrderService serviceOrderService, ChecklistService checklistService)
        {
            _context = context;
            _logger = logger;
            _customerService = customerService;
            _serviceOrderService = serviceOrderService;
            _checklistService = checklistService;
        }

        /// <summary>
        ///     Retrieves and displays a list of all service orders.
        /// </summary>
        ///
        /// <remarks>
        ///     Iterates list to create action anchors for each order, directed at OpenOrder() below.
        /// </remarks>
        /// 
        /// <returns>
        ///     List of service orders to view, or error view if exception occurs.
        /// </returns>
        ///
        /// <exception cref="InvalidOperationException"> thrown from service layer when there is an issue in retrieving service orders. </exception>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var allServiceOrders = await _serviceOrderService.GetAllServiceOrdersAsync();
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

        /// <summary>
        ///     Opens and displays an selected service order
        /// </summary>
        /// 
        /// <remarks>
        ///     Anchor element passes int value derived from orderId as argument to this method.
        ///     Checklist is generated based on 'order.Product' in <see cref="_serviceOrderService.PopulateOrderViewModel(id)"/> 
        /// </remarks>
        /// 
        /// <param name="id"> PK of service order </param>
        /// 
        /// <returns>
        ///     Populated view model (FillOrderViewModel) to the view.
        /// </returns>
        /// 
        /// <exception cref="InvalidOperationException"> thrown from service layer if the order does not exist </exception>
        [HttpGet]
        public async Task<IActionResult> OpenOrder(int id)
        {
            try
            {
                var viewModel = await _serviceOrderService.PopulateOrderViewModel(id);

                return View("OpenOrder", viewModel);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        ///     Updates an order after processing checklist.
        /// </summary>
        /// 
        /// <param name="processedOrder"> Updated View Model: includes Service Order, Checklist and Customer </param>
        /// 
        /// <returns>
        ///     View model to view on success, or error view on exception
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> SaveFilledOrder(FillOrderViewModel processedOrder)
        {
            processedOrder.OrderCompleted = DateTime.Now;

            await _serviceOrderService.UpdateCompletedOrderAsync(processedOrder);

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Preps view for registering an service order
        /// </summary>
        ///
        /// <remarks>
        ///     Initialises new viewmodel to send to the view.
        ///     Populates the <see cref="Viewbag.ExistingCustomers"/> with registered customers from the database;
        ///     this is displayed in drop down menu on the form.
        /// </remarks>>
        /// 
        /// <returns>
        ///     View model to view on success, or error view on exception
        /// </returns>
        /// 
        /// <exception cref="InvalidOperationException"
        ///     thrown from <see cref="_customerService.GetAllCustomersAsync()"/> if no existing customers are found.
        /// </exception>
        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                var viewModel = new OrderViewModel
                {
                    NewOrder = new ServiceOrder(),
                    NewCustomer = new Customer(),
                    NewChecklist = new Checklist()

                {
                        PreparedBy = "N/A",
                        ServiceProcedure = "Standard",
                        
                    }
                };

                viewModel.NewChecklist.ProductType = viewModel.NewOrder.Product;

                var existingCustomers = await _customerService.GetAllCustomersAsync();
                ViewBag.ExistingCustomers = new SelectList(existingCustomers, "CustomerId", "FirstName");
                _logger.LogInformation("Successfully populated data from service into ViewBag, sending to View");
                
                return View("CreateOrder", viewModel);
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

        /// <summary>
        ///     Registers an service order with or without an existing customer
        /// </summary>
        /// 
        /// <param name="viewModel"> Updated model </param>
        /// 
        /// <returns>
        ///     View model to view on valid state && successful order registration <see cref="_serviceOrderService.CreateNewServiceOrderAsync()"/>
        ///     Errorview on invalid state || Redirect index view on unsuccessful call to <see cref="_serviceOrderService.CreateNewServiceOrderAsync()"/>
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderViewModel viewModel)
        {
            var viewModelJson = JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation("CreateOrderViewModel: {ViewModelJson}", viewModelJson);
            if (!ModelState.IsValid)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ViewModel = viewModel,
                    ErrorMessage = "There were some errors with the data you submitted. Please review the data and try again."
                };
                return View("Error", errorViewModel);
            }
            else
            {
                bool isSuccessful = await _serviceOrderService.CreateNewServiceOrderAsync(viewModel);
                if (isSuccessful)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create new service order.");
                    return View(viewModel);
                }
            }
        }

        public async Task<IActionResult> Search(string query)
        {
            if (query == null)
            {
                return View("Index", await _serviceOrderService.GetAllServiceOrdersAsync());
            }
            try
            {
                var searchResults = await _serviceOrderService.Search(query);

                if (searchResults == null)
                {
                    return View("Index", Enumerable.Empty<ServiceOrder>());
                }

                return View("Index", searchResults);
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


        // GET: ServiceOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServiceOrder == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrder.FindAsync(id);
            if (serviceOrder == null)
            {
                return NotFound();
            }
            return View(serviceOrder);
        }

        // POST: ServiceOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,ChecklistId,isActive,OrderRecieved,OrderCompleted,Status,AgreedFinishedDate,ProductName,ProductType,ModelYear,SerialNumber,Warranty,CustomerAgreement,OrderDescription,DiscardedParts,ReplacedPartsReturned,Shipping,WorkHours")] ServiceOrder serviceOrder)
        {
            if (id != serviceOrder.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceOrderExists(serviceOrder.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceOrder);
        }

        // GET: ServiceOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServiceOrder == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrder
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            return View(serviceOrder);
        }

        // POST: ServiceOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServiceOrder == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ServiceOrder'  is null.");
            }
            var serviceOrder = await _context.ServiceOrder.FindAsync(id);
            if (serviceOrder != null)
            {
                _context.ServiceOrder.Remove(serviceOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceOrderExists(int id)
        {
            return (_context.ServiceOrder?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

        // GET: ServiceOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceOrder == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrder
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            return View(serviceOrder);
        }
    }
}
