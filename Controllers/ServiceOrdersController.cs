﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Models;
using Noested.Services;

namespace Noested.Controllers
{
    public class ServiceOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceOrdersController> _logger;
        private readonly CustomerService _customerService;
        private readonly ServiceOrderService _serviceOrderService;

        public ServiceOrdersController(ApplicationDbContext context, ILogger<ServiceOrdersController> logger, CustomerService customerService, ServiceOrderService serviceOrderService)
        {
            _context = context;
            _logger = logger;
            _customerService = customerService;
            _serviceOrderService = serviceOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
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
        
        [HttpGet] // ServiceOrders/CreateOrder
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                var existingCustomers = await _customerService.GetAllCustomersAsync();
                ViewBag.ExistingCustomers = new SelectList(existingCustomers, "CustomerId", "FirstName");
                _logger.LogInformation("Successfully populated data from service into ViewBag, sending to View");
               /* var viewModel = new CreateOrderViewModel
                {
                    NewServiceOrder = new ServiceOrder(),
                    NewCustomer = new Customer()
                };
               */
                return View("CreateOrder");
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
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    if (modelState.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Error in field {modelState.Key}: {modelState.Value.Errors[0].ErrorMessage}");
                    }
                }
                return View("CreateOrder", viewModel);
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