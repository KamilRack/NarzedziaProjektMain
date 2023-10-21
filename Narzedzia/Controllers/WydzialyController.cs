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
	public class WydzialyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WydzialyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wydzialy
        public async Task<IActionResult> Index()
        {
              return View(await _context.Wydzialy.Include(u => u.Uzytkownicy).ToListAsync());
        }

        // GET: Wydzialy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wydzialy == null)
            {
                return NotFound();
            }

            var wydzial = await _context.Wydzialy
                .FirstOrDefaultAsync(m => m.WydzialId == id);
            if (wydzial == null)
            {
                return NotFound();
            }

            return View(wydzial);
        }

        // GET: Wydzialy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wydzialy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WydzialId,NazwaWydzialu,Active")] Wydzial wydzial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wydzial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wydzial);
        }

        // GET: Wydzialy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wydzialy == null)
            {
                return NotFound();
            }

            var wydzial = await _context.Wydzialy.FindAsync(id);
            if (wydzial == null)
            {
                return NotFound();
            }
            return View(wydzial);
        }

        // POST: Wydzialy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WydzialId,NazwaWydzialu,Active")] Wydzial wydzial)
        {
            if (id != wydzial.WydzialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wydzial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WydzialExists(wydzial.WydzialId))
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
            return View(wydzial);
        }

        // GET: Wydzialy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wydzialy == null)
            {
                return NotFound();
            }

            var wydzial = await _context.Wydzialy
                .FirstOrDefaultAsync(m => m.WydzialId == id);
            if (wydzial == null)
            {
                return NotFound();
            }
            if (WydzialNiepusty((int)id))
            {
                ViewBag.DeleteMessage = "Nie można usunąć wybranego elementu, gdyż posiada przypisanych użytkowników.";
            }

            return View(wydzial);
        }

        // POST: Wydzialy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wydzialy == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Wydzialy'  is null.");
            }
            var wydzial = await _context.Wydzialy.FindAsync(id);
            if (wydzial != null)
            {
                _context.Wydzialy.Remove(wydzial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WydzialExists(int id)
        {
          return _context.Wydzialy.Any(e => e.WydzialId == id);
        }

        private bool WydzialNiepusty(int id)
        {
            return (_context.Uzytkownicy?.Any(t => t.WydzialId == id)).GetValueOrDefault();
        }
    }
}
