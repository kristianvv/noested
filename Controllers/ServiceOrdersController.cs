using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Noested.Data;
using Noested.Models;

namespace Noested.Controllers
{
    public class ServiceOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceOrders
        public async Task<IActionResult> Index()
        {
              return _context.ServiceOrder != null ? 
                          View(await _context.ServiceOrder.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ServiceOrder'  is null.");
        }

        // GET: ServiceOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceOrder == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrder
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            return View(serviceOrder);
        }

        // GET: ServiceOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,CustomerID,OrderRecieved,OrderCompleted,OrderStatus,ModelYear,SerialNumber,Warranty,CustomerAgreement,OrderDescription,DiscardedParts,ReplacedPartsReturned,Shipping,IsActive")] ServiceOrder serviceOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceOrder);
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
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,CustomerID,OrderRecieved,OrderCompleted,OrderStatus,ModelYear,SerialNumber,Warranty,CustomerAgreement,OrderDescription,DiscardedParts,ReplacedPartsReturned,Shipping,IsActive")] ServiceOrder serviceOrder)
        {
            if (id != serviceOrder.OrderID)
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
                    if (!ServiceOrderExists(serviceOrder.OrderID))
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
                .FirstOrDefaultAsync(m => m.OrderID == id);
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
          return (_context.ServiceOrder?.Any(e => e.OrderID == id)).GetValueOrDefault();
        }
    }
}
