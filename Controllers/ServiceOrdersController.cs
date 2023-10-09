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
        private readonly NoestedContext _context;

        public ServiceOrdersController(NoestedContext context)
        {
            _context = context;
        }

        // GET: ServiceOrders
        public async Task<IActionResult> Index()
        {
            var noestedContext = _context.ServiceOrder.Include(s => s.Customer);
            return View(await noestedContext.ToListAsync());
        }

        // GET: ServiceOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceOrder == null)
            {
                return NotFound();
            }

            var serviceOrder = await _context.ServiceOrder
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.ServiceOrderID == id);
            if (serviceOrder == null)
            {
                return NotFound();
            }

            return View(serviceOrder);
        }

        // GET: ServiceOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Set<Customer>(), "CustomerID", "CustomerID");
            return View();
        }

        // POST: ServiceOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceOrderID,OrderRecieved,OrderCompleted,SerialNumber,ModelYear,Warranty,CustomerAgreement,RepairDescription,ExpiredParts,WorkHours,ReplacedPartsReturned,ShippingMethod,CustomerID")] ServiceOrder serviceOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Set<Customer>(), "CustomerID", "CustomerID", serviceOrder.CustomerID);
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
            ViewData["CustomerID"] = new SelectList(_context.Set<Customer>(), "CustomerID", "CustomerID", serviceOrder.CustomerID);
            return View(serviceOrder);
        }

        // POST: ServiceOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceOrderID,OrderRecieved,OrderCompleted,SerialNumber,ModelYear,Warranty,CustomerAgreement,RepairDescription,ExpiredParts,WorkHours,ReplacedPartsReturned,ShippingMethod,CustomerID")] ServiceOrder serviceOrder)
        {
            if (id != serviceOrder.ServiceOrderID)
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
                    if (!ServiceOrderExists(serviceOrder.ServiceOrderID))
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
            ViewData["CustomerID"] = new SelectList(_context.Set<Customer>(), "CustomerID", "CustomerID", serviceOrder.CustomerID);
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
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.ServiceOrderID == id);
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
                return Problem("Entity set 'NoestedContext.ServiceOrder'  is null.");
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
          return (_context.ServiceOrder?.Any(e => e.ServiceOrderID == id)).GetValueOrDefault();
        }
    }
}
