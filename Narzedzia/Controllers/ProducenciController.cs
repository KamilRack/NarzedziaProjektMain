using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Narzedzia.Data;
using Narzedzia.Models;

namespace Narzedzia.Controllers
{
	[Authorize(Roles = "admin, nadzor")]
	public class ProducenciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProducenciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Producenci
        public async Task<IActionResult> Index()
        {
              return View(await _context.Producenci.Include(n => n.Narzedzia).ToListAsync());
        }

        // GET: Producenci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Producenci == null)
            {
                return NotFound();
            }

            var producent = await _context.Producenci
                .FirstOrDefaultAsync(m => m.ProducentId == id);
            if (producent == null)
            {
                return NotFound();
            }

            return View(producent);
        }

        // GET: Producenci/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producenci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProducentId,NazwaProducenta,Active")] Producent producent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producent);
        }

        // GET: Producenci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Producenci == null)
            {
                return NotFound();
            }

            var producent = await _context.Producenci.FindAsync(id);
            if (producent == null)
            {
                return NotFound();
            }
            return View(producent);
        }

        // POST: Producenci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProducentId,NazwaProducenta,Active")] Producent producent)
        {
            if (id != producent.ProducentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducentExists(producent.ProducentId))
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
            return View(producent);
        }

        // GET: Producenci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Producenci == null)
            {
                return NotFound();
            }

            var producent = await _context.Producenci
                .FirstOrDefaultAsync(m => m.ProducentId == id);
            if (producent == null)
            {
                return NotFound();
            }
            if (ProducentNiepusty((int)id))
            {
                ViewBag.DeleteMessage = "Nie można usunąć wybranego elementu, gdyż posiada przypisane narzędzia.";
            }

            return View(producent);
        }

        // POST: Producenci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Producenci == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Producenci'  is null.");
            }
            var producent = await _context.Producenci.FindAsync(id);
            if (producent != null)
            {
                _context.Producenci.Remove(producent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducentExists(int id)
        {
          return _context.Producenci.Any(e => e.ProducentId == id);
        }

        private bool ProducentNiepusty(int id)
        {
            return (_context.Narzedzia?.Any(t => t.ProducentId == id)).GetValueOrDefault();
        }
    }
}
