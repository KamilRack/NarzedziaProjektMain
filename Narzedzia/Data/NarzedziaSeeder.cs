using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Cryptography;
using Narzedzia.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Narzedzia.Data
{
    public class NarzedziaSeeder
    {
        public static Random r = new Random();
        public static void Initialize(IServiceProvider serviceProvider) 
        {
            using (var dbContext = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
                
                if (dbContext.Database.CanConnect())
                {
                    SeedRoles(dbContext);
                    SeedStanowisko(dbContext);
                    SeedWydzial(dbContext);
                    SeedKategoria(dbContext);
                    SeedProducent(dbContext);
                    SeedUsers(dbContext);
                    SeedNarzedzie(dbContext);                    
                }
        }

        private static void SeedRoles(ApplicationDbContext dbContext)
        {
            var roleStore = new RoleStore<IdentityRole>(dbContext);
            if (!dbContext.Roles.Any(r => r.Name == "admin"))
            {
                roleStore.CreateAsync(new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "admin"
                }).Wait();
            }
            if (!dbContext.Roles.Any(r => r.Name == "nadzor"))
            {
                roleStore.CreateAsync(new IdentityRole
                {
                    Name = "nadzor",
                    NormalizedName = "nadzor"
                }).Wait();
            }
            if (!dbContext.Roles.Any(r => r.Name == "pracownik"))
            {
                roleStore.CreateAsync(new IdentityRole
                {
                    Name = "pracownik",
                    NormalizedName = "pracownik"
                }).Wait();
            }
        }

        private static void SeedUsers(ApplicationDbContext dbContext)
        {
            if (!dbContext.Users.Any(u => u.UserName == "nadzor@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "nadzor@narzedzia.pl",
                    NormalizedUserName = "nadzor@narzedzia.pl",
                    Email = "nadzor@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Adam",
                    Nazwisko = "Kowalski",
                    NrKontrolny = r.Next(10000,40000),
                    WydzialId = 1,
                    StanowiskoId = 1,
                };

                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "Narzedzia1!");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "nadzor").Wait();
                dbContext.SaveChanges();

            }

            if (!dbContext.Users.Any(u => u.UserName == "nadzor2@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "nadzor2@narzedzia.pl",
                    NormalizedUserName = "nadzor2@narzedzia.pl",
                    Email = "nadzor2@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Jan",
                    Nazwisko = "Zalewski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 2,
                    StanowiskoId = 2,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "Narzedzia1!");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "nadzor").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "konrad@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "konrad@narzedzia.pl",
                    NormalizedUserName = "konrad@narzedzia.pl",
                    Email = "konrad@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Konrad",
                    Nazwisko = "Nowak",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 3,
                    StanowiskoId = 3,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "Narzedzia1!");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "admin").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "arek@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "arek@narzedzia.pl",
                    NormalizedUserName = "arek@narzedzia.pl",
                    Email = "arek@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Arkadiusz",
                    Nazwisko = "Kowalski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 1,
                    StanowiskoId = 1,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "Narzedzia1!");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "admin").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik1@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik1@narzedzia.pl",
                    NormalizedUserName = "pracownik1@narzedzia.pl",
                    Email = "pracownik1@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Aleksander",
                    Nazwisko = "Szewczyk",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 1,
                    StanowiskoId = 1,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik2@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik2@narzedzia.pl",
                    NormalizedUserName = "pracownik2@narzedzia.pl",
                    Email = "pracownik2@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Marcin",
                    Nazwisko = "Kamiński",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 1,
                    StanowiskoId = 2,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik3@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik3@narzedzia.pl",
                    NormalizedUserName = "pracownik3@narzedzia.pl",
                    Email = "pracownik3@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Rafał",
                    Nazwisko = "Maciejewski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 1,
                    StanowiskoId = 3,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik4@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik4@narzedzia.pl",
                    NormalizedUserName = "pracownik4@narzedzia.pl",
                    Email = "pracownik4@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Leszek",
                    Nazwisko = "Michalak",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 1,
                    StanowiskoId = 4,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik5@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik5@narzedzia.pl",
                    NormalizedUserName = "pracownik5@narzedzia.pl",
                    Email = "pracownik5@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Dariusz",
                    Nazwisko = "Lewandowski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 2,
                    StanowiskoId = 5,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik6@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik6@narzedzia.pl",
                    NormalizedUserName = "pracownik6@narzedzia.pl",
                    Email = "pracownik6@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Roman",
                    Nazwisko = "Kozłowski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 2,
                    StanowiskoId = 6,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik7@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik7@narzedzia.pl",
                    NormalizedUserName = "pracownik7@narzedzia.pl",
                    Email = "pracownik7@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Natan",
                    Nazwisko = "Szewczyk",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 2,
                    StanowiskoId = 7,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik8@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik8@narzedzia.pl",
                    NormalizedUserName = "pracownik8@narzedzia.pl",
                    Email = "pracownik8@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Adrian",
                    Nazwisko = "Jaworski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 2,
                    StanowiskoId = 8,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik9@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik9@narzedzia.pl",
                    NormalizedUserName = "pracownik9@narzedzia.pl",
                    Email = "pracownik9@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Robert",
                    Nazwisko = "Kowalski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 3,
                    StanowiskoId = 9,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik10@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik10@narzedzia.pl",
                    NormalizedUserName = "pracownik10@narzedzia.pl",
                    Email = "pracownik10@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Dorian",
                    Nazwisko = "Zieliński",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 3,
                    StanowiskoId = 1,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik11@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik11@narzedzia.pl",
                    NormalizedUserName = "pracownik11@narzedzia.pl",
                    Email = "pracownik11@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Rafał",
                    Nazwisko = "Ziółkowski",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 3,
                    StanowiskoId = 2,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

            if (!dbContext.Users.Any(u => u.UserName == "pracownik12@narzedzia.pl"))
            {
                var uzytkownik = new Uzytkownik
                {
                    UserName = "pracownik12@narzedzia.pl",
                    NormalizedUserName = "pracownik12@narzedzia.pl",
                    Email = "pracownik12@narzedzia.pl",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    Imie = "Hubert",
                    Nazwisko = "Walczak",
                    NrKontrolny = r.Next(10000, 40000),
                    WydzialId = 3,
                    StanowiskoId = 3,
                };
                var password = new PasswordHasher<Uzytkownik>();
                var hashed = password.HashPassword(uzytkownik, "1qaz@WSX");
                uzytkownik.PasswordHash = hashed;
                var userStore = new UserStore<Uzytkownik>(dbContext);
                userStore.CreateAsync(uzytkownik).Wait();
                userStore.AddToRoleAsync(uzytkownik, "pracownik").Wait();
                dbContext.SaveChanges();
            }

        }

        private static void SeedKategoria(ApplicationDbContext dbContext)
        {
            if (!dbContext.Kategorie.Any())
            {
                var kat = new List<Kategoria>
                {
                    new Kategoria {NazwaKategorii = "Frezarka", Active = true},
                    new Kategoria {NazwaKategorii = "Klucz akumulatorowy", Active = true},
                    new Kategoria {NazwaKategorii = "Klucz dynamometryczny", Active = true},
                    new Kategoria {NazwaKategorii = "Klucz pneumatyczny", Active = true},
                    new Kategoria {NazwaKategorii = "Klucz udarowy", Active = true},
                    new Kategoria {NazwaKategorii = "Lutownica", Active = true},
                    new Kategoria {NazwaKategorii = "Pilarka", Active = true},
                    new Kategoria {NazwaKategorii = "Polerka", Active = true},
                    new Kategoria {NazwaKategorii = "Spawarka", Active = true},
                    new Kategoria {NazwaKategorii = "Szlifierka", Active = true},
                };

                dbContext.AddRange(kat);
                dbContext.SaveChanges();
            }
        }

        private static void SeedProducent(ApplicationDbContext dbContext)
        {
            if (!dbContext.Producenci.Any())
            {
                var kat = new List<Producent>
                {
                    new Producent {NazwaProducenta = "Bahco", Active = true},
                    new Producent {NazwaProducenta = "Bosch", Active = true},
                    new Producent {NazwaProducenta = "DeWalt", Active = true},
                    new Producent {NazwaProducenta = "Dremel", Active = true},
                    new Producent {NazwaProducenta = "Einhell", Active = true},
                    new Producent {NazwaProducenta = "Festool", Active = true},
                    new Producent {NazwaProducenta = "Fischer", Active = true},
                    new Producent {NazwaProducenta = "Fiskars", Active = true},
                    new Producent {NazwaProducenta = "Graphite", Active = true},
                    new Producent {NazwaProducenta = "Hitachi", Active = true},
                    new Producent {NazwaProducenta = "Makita", Active = true},
                    new Producent {NazwaProducenta = "Metabo", Active = true},
                    new Producent {NazwaProducenta = "Milwaukee", Active = true},
                    new Producent {NazwaProducenta = "Neo", Active = true},
                    new Producent {NazwaProducenta = "Proxxon", Active = true},
                    new Producent {NazwaProducenta = "Ryobi", Active = true},
                    new Producent {NazwaProducenta = "Stanley", Active = true},
                    new Producent {NazwaProducenta = "Uryu", Active = true},
                    new Producent {NazwaProducenta = "Wiha", Active = true},
                    new Producent {NazwaProducenta = "Yato", Active = true},
                };

                dbContext.AddRange(kat);
                dbContext.SaveChanges();
            }
        }

        private static void SeedNarzedzie(ApplicationDbContext dbContext)
        {
            if (!dbContext.Narzedzia.Any())
            {
                var kat = new List<Narzedzie>();

                for (int i = 1; i < 55; i++)
                {
                    kat.Add(new Narzedzie { ProducentId = r.Next(1, 21), KategoriaId = r.Next(1, 11), DataPrzyjecia = DateTime.Now, UzytkownikId = LosowyUser(dbContext), NumerNarzedzia = r.Next(10000, 50000), Nazwa = "Narzędzie testowe #" + i, Status = Status.używane });
                }
                dbContext.AddRange(kat);
                dbContext.SaveChanges();
            }
        }

        private static void SeedWydzial(ApplicationDbContext dbContext)
        {
            if (!dbContext.Wydzialy.Any())
            {
                var kat = new List<Wydzial>
                {
                    new Wydzial {NazwaWydzialu = "P5", Active = true},
                    new Wydzial {NazwaWydzialu = "P6", Active = true},
                    new Wydzial {NazwaWydzialu = "P7", Active = true},
                };

                dbContext.AddRange(kat);
                dbContext.SaveChanges();
            }
        }

        private static void SeedStanowisko(ApplicationDbContext dbContext)
        {
            if (!dbContext.Stanowiska.Any())
            {
                var kat = new List<Stanowisko>
                {
                    new Stanowisko {NazwaStanowiska = "A", Active = true},
                    new Stanowisko {NazwaStanowiska = "B", Active = true},
                    new Stanowisko {NazwaStanowiska = "C", Active = true},
                    new Stanowisko {NazwaStanowiska = "D", Active = true},
                    new Stanowisko {NazwaStanowiska = "E", Active = true},
                    new Stanowisko {NazwaStanowiska = "F", Active = true},
                    new Stanowisko {NazwaStanowiska = "G", Active = true},
                    new Stanowisko {NazwaStanowiska = "H", Active = true},
                    new Stanowisko {NazwaStanowiska = "I", Active = true},
                };

                dbContext.AddRange(kat);
                dbContext.SaveChanges();
            }
        }

        public static string LosowyUser(ApplicationDbContext dbContext)
        {
            var user = dbContext.Uzytkownicy.OrderBy(o => Guid.NewGuid()).First();
            return user.Id.ToString();

        }
    }
}
