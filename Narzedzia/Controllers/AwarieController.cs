using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Narzedzia.Data;
using Narzedzia.Models;
using Microsoft.AspNetCore.SignalR;


namespace Narzedzia.Controllers
{
    
    public class AwarieController : Controller
    {
        private readonly ApplicationDbContext _context;


        public AwarieController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin,nadzor")]
        // GET: Awarie
        public async Task<IActionResult> Index()
        {

            var adminRoleId = await _context.Roles.Where(r => r.NormalizedName == "admin").Select(r => r.Id).FirstOrDefaultAsync();
            var nadzorRoleId = await _context.Roles.Where(r => r.NormalizedName == "nadzor").Select(r => r.Id).FirstOrDefaultAsync();

            var applicationDbContext = _context.Awarie.Include(a => a.Narzedzie).Include(a => a.Uzytkownicy).Include(a => a.UzytkownikRealizujacy);
            // var userList = await _context.Uzytkownicy
            //   .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && (ur.RoleId == "adminRoleId" || ur.RoleId == "nadzorRoleId")))
            //  .ToListAsync();

            var userList = await _context.Users
        .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && (ur.RoleId == adminRoleId || ur.RoleId == nadzorRoleId)))
        .ToListAsync();
            var userSelectList = new SelectList(userList, "Id", "Email");


            ViewData["UserList"] = userSelectList;

         
            return View(await applicationDbContext.ToListAsync());



            //  var applicationDbContext = _context.Awarie.Include(a => a.Narzedzie).Include(a => a.Uzytkownicy).Include(a => a.UzytkownikRealizujacy);
            // var userList = await _context.Uzytkownicy.ToListAsync();
            // ViewData["UserList"] = new SelectList(userList, "Id", "Email");

            //  return View(await applicationDbContext.ToListAsync());

        }

        [HttpPost]
        public IActionResult ZapiszUzytkownika(int awariaId, string selectedUserId)
        {
            try
            {
                // Pobierz rekord z bazy danych na podstawie awariaId
                var awaria = _context.Awarie.FirstOrDefault(a => a.IdAwaria == awariaId);

                if (awaria != null)
                {
                    // Zaktualizuj pole UzytkownikRealizujacyId na podstawie selectedUserId
                    awaria.UzytkownikRealizujacyId = selectedUserId;

                    // Zapisz zmiany w bazie danych
                    _context.SaveChanges();

                    return Json(new { success = true });
                }

                return Json(new { success = false, error = "Nie znaleziono rekordu o podanym identyfikatorze awarii." });
            }
            catch (Exception ex)
            {
                // Obsłuż ewentualny błąd
                return Json(new { success = false, error = ex.Message });
            }
        }

      

        [Authorize(Roles = "admin,nadzor")]

        // GET: Awarie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Awarie == null)
            {
                return NotFound();
            }

            var awaria = await _context.Awarie
                .Include(a => a.Narzedzie)
                .Include(a => a.Uzytkownicy)
                .Include(a => a.UzytkownikRealizujacy)
                .FirstOrDefaultAsync(m => m.IdAwaria == id);
            ViewBag.StatusAwaria = awaria.Status;

          //  if (awaria == null)
          //  {
          //      return NotFound();
          //  }
            //
            // Dodaj kod, który zaktualizuje awarię na podstawie danych w bazie danych
            var updatedAwaria = await _context.Awarie
                .Include(a => a.Narzedzie)
                .Include(a => a.Uzytkownicy)
                .Include(a => a.UzytkownikRealizujacy)
                .FirstOrDefaultAsync(m => m.IdAwaria == id);

            if (updatedAwaria != null)
            {
                awaria = updatedAwaria;
            }
            //
            ViewBag.StatusAwaria = awaria.Status;
            if (awaria == null)
            {
                return NotFound();
            }
            return View(awaria);
        }

        // GET: Awarie/Create
        public IActionResult Create()
        {
            ViewData["NarzedzieId"] = new SelectList(_context.Narzedzia, "NarzedzieId", "Nazwa");
         //   ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Email");
            return View();
        }

        // POST: Awarie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAwaria,NarzedzieId,DescriptionAwaria,NumberAwaria")] Awaria awaria)
        //public async Task<IActionResult> Create([Bind("IdAwaria,NarzedzieId,DescriptionAwaria,NumberAwaria,DataPrzyjecia,UzytkownikId,Status")] Awaria awaria)

        {
            if (ModelState.IsValid)
            {
                awaria.DataPrzyjecia = DateTime.Now;
                awaria.UzytkownikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                awaria.Status = StatusAwaria.nowe;
                _context.Add(awaria);
                await _context.SaveChangesAsync();

                if (User.IsInRole("pracownik"))
                {
                    // Przekieruj użytkownika do akcji "Index" kontrolera "Home"
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction(nameof(Index));
            }
            
             ViewData["NarzedzieId"] = new SelectList(_context.Narzedzia, "NarzedzieId", "Nazwa", awaria.NarzedzieId);
            // ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Email", awaria.UzytkownikId);
            ViewData["UzytkownikId"] = awaria.Uzytkownicy;
               return View(awaria);
        }
        [Authorize(Roles = "admin,nadzor")]
        // GET: Awarie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Awarie == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var awaria = await _context.Awarie.Include(n => n.Uzytkownicy).FirstOrDefaultAsync(m => m.IdAwaria == id);
            if (awaria == null)
            {
                return NotFound();
            }


            ViewData["NarzedzieId"] = new SelectList(_context.Narzedzia, "NarzedzieId", "Nazwa", awaria.NarzedzieId);
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Email", awaria.UzytkownikId);
            ViewData["UzytkownikRealizujacyId"] = new SelectList(_context.Uzytkownicy, "Id", "Email", awaria.UzytkownikRealizujacyId);
            if (awaria.UzytkownikId == null)
            {
                ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Email");
            }
            else
            {
                ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Email", awaria.UzytkownikId);
                ViewData["Obecny"] = awaria.Uzytkownicy?.Email;
                ViewData["ObecnyId"] = awaria.UzytkownikId;
            }

            //
            var updatedAwaria = await _context.Awarie
       .Include(a => a.Narzedzie)
       .Include(a => a.Uzytkownicy)
       .Include(a => a.UzytkownikRealizujacy)
       .FirstOrDefaultAsync(m => m.IdAwaria == id);

            if (updatedAwaria != null)
            {
                awaria = updatedAwaria;
            }
            //
            return View(awaria);
        }
        [Authorize(Roles = "admin,nadzor")]
        // POST: Awarie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAwaria,NarzedzieId,DescriptionAwaria,NumberAwaria,DataPrzyjecia,UzytkownikId,Status,UzytkownikRealizujacyId,NotatkaTechniczna")] Awaria awaria, string? obecny)
        {
            if (id != awaria.IdAwaria)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingAwaria = await _context.Awarie
        .Include(a => a.Uzytkownicy)
        .FirstOrDefaultAsync(m => m.IdAwaria == id && m.UzytkownikId == userId);

            if (existingAwaria == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!(obecny == null))
                    {
                        awaria.UzytkownikId = obecny;
                    }
                    _context.Update(awaria);
                    await _context.SaveChangesAsync();

                    // Po zapisaniu zmiany, wczytaj zaktualizowaną awarię z bazy danych
                    awaria = await _context.Awarie
                        .Include(a => a.Narzedzie)
                        .Include(a => a.Uzytkownicy)
                        .Include(a => a.UzytkownikRealizujacy)
                        .FirstOrDefaultAsync(m => m.IdAwaria == id);



                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwariaExists(awaria.IdAwaria))
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
            ViewData["NarzedzieId"] = new SelectList(_context.Narzedzia, "NarzedzieId", "NarzedzieId", awaria.NarzedzieId);
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownicy, "Id", "Id", awaria.UzytkownikId);
            ViewData["UzytkownikRealizujacyId"] = new SelectList(_context.Uzytkownicy, "Id", "Id", awaria.UzytkownikId);

            return View(awaria);
        }

        [Authorize(Roles = "admin,nadzor")]
        // GET: Awarie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Awarie == null)
            {
                return NotFound();
            }

       


            var awaria = await _context.Awarie
                .Include(a => a.Narzedzie)
                .Include(a => a.Uzytkownicy)
                .FirstOrDefaultAsync(m => m.IdAwaria == id);
            if (awaria == null)
            {
                return NotFound();
            }



            return View(awaria);

   

        }
        [Authorize(Roles = "admin,nadzor")]
        // POST: Awarie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Awarie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Awarie'  is null.");
            }
            var awaria = await _context.Awarie.FindAsync(id);
            if (awaria != null)
            {
                _context.Awarie.Remove(awaria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AwariaExists(int id)
        {
          return (_context.Awarie?.Any(e => e.IdAwaria == id)).GetValueOrDefault();
        }
    }

}
