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
    public class NarzedziaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NarzedziaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Narzedzia
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Narzedzia.Include(n => n.Kategorie).Include(n => n.Producenci).Include(n => n.Uzytkownicy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Narzedzia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Narzedzia == null)
            {
                return NotFound();
            }

            var narzedzie = await _context.Narzedzia
                .Include(n => n.Kategorie)
                .Include(n => n.Producenci)
                .Include(n => n.Uzytkownicy)
                .FirstOrDefaultAsync(m => m.NarzedzieId == id);
            ViewBag.Status = narzedzie.Status;
            if (narzedzie == null)
            {
                return NotFound();
            }

            return View(narzedzie);
        }

        // GET: Narzedzia/Create
        public IActionResult Create()
        {
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie.Where(c => c.Active == true), "KategoriaId", "NazwaKategorii");
            ViewData["ProducentId"] = new SelectList(_context.Producenci.Where(c => c.Active == true), "ProducentId", "NazwaProducenta");
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id");
            return View();
        }

        // POST: Narzedzia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NarzedzieId,ProducentId,KategoriaId,DataPrzyjecia,UzytkownikId,NumerNarzedzia,Nazwa,Status")] Narzedzie narzedzie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(narzedzie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "KategoriaId", "NazwaKategorii", narzedzie.KategoriaId);
            ViewData["ProducentId"] = new SelectList(_context.Producenci, "ProducentId", "NazwaProducenta", narzedzie.ProducentId);
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id", narzedzie.UzytkownikId);
            return View(narzedzie);
        }

        // GET: Narzedzia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Narzedzia == null)
            {
                return NotFound();
            }

            var narzedzie = await _context.Narzedzia.Include(n => n.Uzytkownicy).FirstOrDefaultAsync(m => m.NarzedzieId == id);
            if (narzedzie == null)
            {
                return NotFound();
            }
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "KategoriaId", "NazwaKategorii", narzedzie.KategoriaId);
            ViewData["ProducentId"] = new SelectList(_context.Producenci, "ProducentId", "NazwaProducenta", narzedzie.ProducentId);
            if (narzedzie.UzytkownikId == null)
            {
                ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Imie_Nazwisko");
            } else
            {
                ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Imie_Nazwisko", narzedzie.UzytkownikId);
                ViewData["Obecny"] = narzedzie.Uzytkownicy?.Imie_Nazwisko;
                ViewData["ObecnyId"] = narzedzie.UzytkownikId;
            }
            return View(narzedzie);
        }

        // POST: Narzedzia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NarzedzieId,ProducentId,KategoriaId,DataPrzyjecia,NumerNarzedzia,Nazwa,Status,UzytkownikId")] Narzedzie narzedzie, string? obecny)
        {
            if (id != narzedzie.NarzedzieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!(obecny == null))
                    {
                        narzedzie.UzytkownikId = obecny;
                    }
                    _context.Update(narzedzie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NarzedzieExists(narzedzie.NarzedzieId))
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
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "KategoriaId", "NazwaKategorii", narzedzie.KategoriaId);
            ViewData["ProducentId"] = new SelectList(_context.Producenci, "ProducentId", "NazwaProducenta", narzedzie.ProducentId);
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id", narzedzie.UzytkownikId);
            return View(narzedzie);
        }

        // GET: Narzedzia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Narzedzia == null)
            {
                return NotFound();
            }

            var narzedzie = await _context.Narzedzia
                .Include(n => n.Kategorie)
                .Include(n => n.Producenci)
                .Include(n => n.Uzytkownicy)
                .FirstOrDefaultAsync(m => m.NarzedzieId == id);
            if (narzedzie == null)
            {
                return NotFound();
            }
            if (!NarzedzieZlikwidowane((int)id))
            {
                ViewBag.DeleteMessage = "Nie można usunąć wybranego elementu, gdyż nie został wcześniej zlikwidowany (zmiana statusu).";
            }

            return View(narzedzie);
        }

        // POST: Narzedzia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Narzedzia == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Narzedzia'  is null.");
            }
            var narzedzie = await _context.Narzedzia.FindAsync(id);
            if (narzedzie != null)
            {
                _context.Narzedzia.Remove(narzedzie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NarzedzieExists(int id)
        {
          return _context.Narzedzia.Any(e => e.NarzedzieId == id);
        }

        private bool NarzedzieZlikwidowane(int id)
        {
            var status =  _context.Narzedzia.FirstOrDefault(x => x.NarzedzieId == id).Status;
            if (status == Status.zlikwidowane) { return true; }
            return false;
        }
    }
}
