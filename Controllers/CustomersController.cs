using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakilaApp.Data;
using SakilaApp.Models;

namespace SakilaApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly SakilaContext _context;

        public CustomersController(SakilaContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var sakilaContext = _context.Customers.Include(c => c.Address).Include(c => c.Store);
            return View(await sakilaContext.ToListAsync());
        }


/*


// RETO 9 — Total de clientes
// SQL: SELECT COUNT(*) FROM customer;
public async Task<IActionResult> Index()
{
    var total = await _context.Customers.CountAsync();
    ViewBag.Total = total;
    return View(await _context.Customers.ToListAsync());
}


// RETO 10 — Clientes por apellido
// SQL: SELECT * FROM customer WHERE last_name ILIKE '%SMI%';
public async Task<IActionResult> Index()
{
    var customers = await _context.Customers
        .Where(c => c.LastName.Contains("SMI"))
        .ToListAsync();
    return View(customers);
}


// RETO 12 — Top 10 clientes con más alquileres
// SQL: SELECT customer_id, COUNT(*) FROM rental GROUP BY customer_id ORDER BY COUNT(*) DESC LIMIT 10;
public async Task<IActionResult> Index()
{
    var customers = await _context.Rentals
        .GroupBy(r => r.CustomerId)
        .Select(g => new {
            CustomerId = g.Key,
            TotalAlquileres = g.Count()
        })
        .OrderByDescending(x => x.TotalAlquileres)
        .Take(10)
        .ToListAsync();
    return View(customers);
}



*/


        

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.Store)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,StoreId,FirstName,LastName,Email,AddressId,Activebool,CreateDate,LastUpdate,Active")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", customer.AddressId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId", customer.StoreId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", customer.AddressId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId", customer.StoreId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,StoreId,FirstName,LastName,Email,AddressId,Activebool,CreateDate,LastUpdate,Active")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId", customer.AddressId);
            ViewData["StoreId"] = new SelectList(_context.Stores, "StoreId", "StoreId", customer.StoreId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Address)
                .Include(c => c.Store)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
