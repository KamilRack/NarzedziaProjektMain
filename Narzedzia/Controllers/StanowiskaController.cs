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
	public class StanowiskaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StanowiskaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stanowiska
        public async Task<IActionResult> Index()
        {
              return View(await _context.Stanowiska.Include(u => u.Uzytkownicy).ToListAsync());
        }

        // GET: Stanowiska/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stanowiska == null)
            {
                return NotFound();
            }

            var stanowisko = await _context.Stanowiska
                .FirstOrDefaultAsync(m => m.StanowiskoId == id);
            if (stanowisko == null)
            {
                return NotFound();
            }

            return View(stanowisko);
        }

        // GET: Stanowiska/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stanowiska/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StanowiskoId,NazwaStanowiska,Active")] Stanowisko stanowisko)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stanowisko);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stanowisko);
        }

        // GET: Stanowiska/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stanowiska == null)
            {
                return NotFound();
            }

            var stanowisko = await _context.Stanowiska.FindAsync(id);
            if (stanowisko == null)
            {
                return NotFound();
            }
            return View(stanowisko);
        }

        // POST: Stanowiska/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StanowiskoId,NazwaStanowiska,Active")] Stanowisko stanowisko)
        {
            if (id != stanowisko.StanowiskoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stanowisko);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StanowiskoExists(stanowisko.StanowiskoId))
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
            return View(stanowisko);
        }

        // GET: Stanowiska/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stanowiska == null)
            {
                return NotFound();
            }

            var stanowisko = await _context.Stanowiska
                .FirstOrDefaultAsync(m => m.StanowiskoId == id);
            if (stanowisko == null)
            {
                return NotFound();
            }
            if (StanowiskoNiepuste((int)id))
            {
                ViewBag.DeleteMessage = "Nie można usunąć wybranego elementu, gdyż posiada przypisanych użytkowników.";
            }

            return View(stanowisko);
        }

        // POST: Stanowiska/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stanowiska == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stanowiska'  is null.");
            }
            var stanowisko = await _context.Stanowiska.FindAsync(id);
            if (stanowisko != null)
            {
                _context.Stanowiska.Remove(stanowisko);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StanowiskoExists(int id)
        {
          return _context.Stanowiska.Any(e => e.StanowiskoId == id);
        }

        private bool StanowiskoNiepuste(int id)
        {
            return (_context.Uzytkownicy?.Any(t => t.StanowiskoId == id)).GetValueOrDefault();
        }
    }
}
