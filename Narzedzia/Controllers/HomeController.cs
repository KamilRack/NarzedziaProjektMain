using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Narzedzia.Data;
using Narzedzia.Models;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace Narzedzia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult PobierzZgloszenia()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Sprawdź, czy użytkownik ma rolę "admin"
            if (User.IsInRole("admin") || User.IsInRole("nadzor"))

            {
                // Dla administratora wyświetl zgłoszenia na podstawie UzytkownikRealizujacyId
                var zgloszenia = _context.Awarie
                    .Where(a => a.UzytkownikRealizujacyId == userId)
                    .Include(a => a.Narzedzie)
                    .Include(a => a.Uzytkownicy)
                    .ToList();

                var viewModel = new Tuple<List<Narzedzie>, List<Awaria>>(new List<Narzedzie>(), zgloszenia);

                return PartialView("_AwariePartialView", viewModel);
            }
            else if (User.IsInRole("pracownik"))
            {
                // Dla pracownika wyświetl zgłoszenia na podstawie UzytkownikId
                var zgloszenia = _context.Awarie
                    .Where(a => a.UzytkownikId == userId)
                    .Include(a => a.Narzedzie)
                    .Include(a => a.Uzytkownicy)
                    .ToList();

                var viewModel = new Tuple<List<Narzedzie>, List<Awaria>>(new List<Narzedzie>(), zgloszenia);

                return PartialView("_AwariePartialView", viewModel);
            }

            // Inna logika lub błąd dla innych przypadków
            return View("Error");
        }



        public IActionResult PobierzNowaStrone()
        {
            return View("Index"); // Zwraca widok Index.cshtml
        }
        public IActionResult Index()
        {



            if (User.IsInRole("admin") || User.IsInRole("nadzor"))
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // var narzedzia = _context.Narzedzia.Include(n => n.Kategorie).Include(n => n.Producenci).Include(n => n.Uzytkownicy).ToList();
                // ViewBag.Przyjete = narzedzia.Where(x => x.Status == Status.przyjęte).Count();
                // ViewBag.Uzywane = narzedzia.Where(x => x.Status == Status.używane).Count();
                // ViewBag.Naprawiane = narzedzia.Where(x => x.Status == Status.naprawiane).Count();
                // ViewBag.Zlikwidowane = narzedzia.Where(x => x.Status == Status.zlikwidowane).Count();      
                //  return View(narzedzia);
                var narzedzia = _context.Narzedzia
         .Include(n => n.Kategorie)
         .Include(n => n.Producenci)
         .Include(n => n.Uzytkownicy)
         .ToList();


                var awarie = _context.Awarie
                                    .Include(n => n.Narzedzie).Include(n => n.Uzytkownicy)
                                    .Where(x => x.UzytkownikRealizujacyId == userId).ToList();

                ViewBag.Przyjete = narzedzia.Where(x => x.Status == Status.przyjęte).Count();
                ViewBag.Uzywane = narzedzia.Where(x => x.Status == Status.używane).Count();
                ViewBag.Naprawiane = narzedzia.Where(x => x.Status == Status.naprawiane).Count();
                ViewBag.Zlikwidowane = narzedzia.Where(x => x.Status == Status.zlikwidowane).Count();

                var viewModel = new Tuple<List<Narzedzie>, List<Awaria>>(narzedzia, awarie);
                return View("Index", viewModel); // Tu przekazujemy model do widoku Index
            }
            if (User.IsInRole("pracownik"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var narzedzia = _context.Narzedzia
                    .Include(n => n.Kategorie)
                    .Include(n => n.Producenci)
                    .Include(n => n.Uzytkownicy)
                    .Where(x => x.UzytkownikId == userId)
                    .ToList();

                var awarie = _context.Awarie
                    .Include(n => n.Narzedzie) .Include(n => n.Uzytkownicy) 
                    .Where(x => x.UzytkownikId == userId && x.Status != StatusAwaria.zakończone).ToList();

                ViewBag.Przyjete = narzedzia.Where(x => x.Status == Status.przyjęte).Count();
                ViewBag.Uzywane = narzedzia.Where(x => x.Status == Status.używane).Count();
                ViewBag.Naprawiane = narzedzia.Where(x => x.Status == Status.naprawiane).Count();
                ViewBag.Zlikwidowane = narzedzia.Where(x => x.Status == Status.zlikwidowane).Count();
                ViewBag.ImieNazwisko = _context.Uzytkownicy.Where(x => x.Id == userId).Select(x => x.Imie_Nazwisko).FirstOrDefault();

                var viewModel = new Tuple<List<Narzedzie>, List<Awaria>>(narzedzia, awarie);
                return View("Index", viewModel); 
                // Tu przekazujemy model do widoku Index
                                                 //  var awarie = _context.Awarie.ToList();
                                                 //  var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                                 //  var narzedzia = _context.Narzedzia.Include(n => n.Kategorie).Include(n => n.Producenci).Include(n => n.Uzytkownicy).Where(x => x.UzytkownikId == userId);
                                                 //  ViewBag.Przyjete = narzedzia.Where(x => x.Status == Status.przyjęte).Count();
                                                 //  ViewBag.Uzywane = narzedzia.Where(x => x.Status == Status.używane).Count();
                                                 //  ViewBag.Naprawiane = narzedzia.Where(x => x.Status == Status.naprawiane).Count();
                                                 //  ViewBag.Zlikwidowane = narzedzia.Where(x => x.Status == Status.zlikwidowane).Count();
                                                 //ViewBag.ImieNazwisko = _context.Uzytkownicy.Where(x => x.Id == userId).Select(x => x.Imie_Nazwisko).FirstOrDefault();
                                                 //  var viewModel = new Tuple<List<Narzedzie>, List<Awaria>>(narzedzia, awarie);
                                                 //  return View(narzedzia);


            }


            return View();


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}