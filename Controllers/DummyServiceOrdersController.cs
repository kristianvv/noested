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
    public class DummyServiceOrdersController : Controller
    {
        private readonly NoestedContext _context;

        public DummyServiceOrdersController(NoestedContext context)
        {
            _context = context;
        }

        // GET: DummyServiceOrders
        public async Task<IActionResult> Index()
        {
              return _context.DummyServiceOrder != null ? 
                          View(await _context.DummyServiceOrder.ToListAsync()) :
                          Problem("Entity set 'NoestedContext.DummyServiceOrder'  is null.");
        }

        // GET: DummyServiceOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DummyServiceOrder == null)
            {
                return NotFound();
            }

            var dummyServiceOrder = await _context.DummyServiceOrder
                .FirstOrDefaultAsync(m => m.ServiceOrderID == id);
            if (dummyServiceOrder == null)
            {
                return NotFound();
            }

            return View(dummyServiceOrder);
        }

        // GET: DummyServiceOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DummyServiceOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceOrderID,OrderNum,OrderDate,CustomerFirstname,CustomerLastname")] DummyServiceOrder dummyServiceOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dummyServiceOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dummyServiceOrder);
        }

        // GET: DummyServiceOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DummyServiceOrder == null)
            {
                return NotFound();
            }

            var dummyServiceOrder = await _context.DummyServiceOrder.FindAsync(id);
            if (dummyServiceOrder == null)
            {
                return NotFound();
            }
            return View(dummyServiceOrder);
        }

        // POST: DummyServiceOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceOrderID,OrderNum,OrderDate,CustomerFirstname,CustomerLastname")] DummyServiceOrder dummyServiceOrder)
        {
            if (id != dummyServiceOrder.ServiceOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dummyServiceOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DummyServiceOrderExists(dummyServiceOrder.ServiceOrderID))
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
            return View(dummyServiceOrder);
        }

        // GET: DummyServiceOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DummyServiceOrder == null)
            {
                return NotFound();
            }

            var dummyServiceOrder = await _context.DummyServiceOrder
                .FirstOrDefaultAsync(m => m.ServiceOrderID == id);
            if (dummyServiceOrder == null)
            {
                return NotFound();
            }

            return View(dummyServiceOrder);
        }

        // POST: DummyServiceOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DummyServiceOrder == null)
            {
                return Problem("Entity set 'NoestedContext.DummyServiceOrder'  is null.");
            }
            var dummyServiceOrder = await _context.DummyServiceOrder.FindAsync(id);
            if (dummyServiceOrder != null)
            {
                _context.DummyServiceOrder.Remove(dummyServiceOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DummyServiceOrderExists(int id)
        {
          return (_context.DummyServiceOrder?.Any(e => e.ServiceOrderID == id)).GetValueOrDefault();
        }
    }
}
