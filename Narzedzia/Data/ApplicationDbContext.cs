using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Narzedzia.Models;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace Narzedzia.Data
{
    public class ApplicationDbContext : IdentityDbContext<Uzytkownik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Narzedzie> Narzedzia { get; set; }
        public DbSet<Producent> Producenci { get; set; }
        public DbSet<Wydzial> Wydzialy { get; set; }
        public DbSet<Stanowisko> Stanowiska { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }

        public DbSet<Awaria> Awarie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Awaria>()
           .HasOne(a => a.Narzedzie) // Awaria ma jedno Narzedzie
           .WithMany(n => n.Awarie) // Narzedzie może mieć wiele Awarie
           .HasForeignKey(a => a.NarzedzieId) // Klucz obcy NarzedzieId w tabeli Awaria
           .OnDelete(DeleteBehavior.Restrict); // Opcjonalnie, ustal sposób usuwania rekordów

            modelbuilder.Entity<Awaria>()
                .HasOne(a => a.Uzytkownicy) // Awaria ma jednego Uzytkownika
                .WithMany(u => u.Awarie) // Uzytkownik może mieć wiele Awarie
                .HasForeignKey(a => a.UzytkownikId) // Klucz obcy UzytkownikId w tabeli Awaria
                .OnDelete(DeleteBehavior.Restrict); // Opcjonalnie, ustal sposób usuwania rekordów
        

        modelbuilder.Entity<Uzytkownik>()
                .HasMany(c => c.Narzedzia)
                .WithOne(t => t.Uzytkownicy);

            modelbuilder.Entity<Uzytkownik>()
                .HasOne(c => c.Wydzialy)
                .WithMany(t => t.Uzytkownicy);

            modelbuilder.Entity<Uzytkownik>()
                .HasOne(c => c.Stanowiska)
                .WithMany(t => t.Uzytkownicy);

            modelbuilder.Entity<Narzedzie>()
                .HasOne(c => c.Producenci)
                .WithMany(t => t.Narzedzia);

            modelbuilder.Entity<Narzedzie>()
                .HasOne(c => c.Kategorie)
                .WithMany(t => t.Narzedzia);

            modelbuilder.Entity<Narzedzie>()
                .HasOne(c => c.Uzytkownicy)
                .WithMany(t => t.Narzedzia);

            modelbuilder.Entity<Kategoria>()
                .HasMany(c => c.Narzedzia)
                .WithOne(t => t.Kategorie);

            modelbuilder.Entity<Producent>()
                .HasMany(c => c.Narzedzia)
                .WithOne(t => t.Producenci);

            modelbuilder.Entity<Wydzial>()
                .HasMany(c => c.Uzytkownicy)
                .WithOne(t => t.Wydzialy);

            modelbuilder.Entity<Stanowisko>()
                .HasMany(c => c.Uzytkownicy)
                .WithOne(t => t.Stanowiska);

        }
    }
}