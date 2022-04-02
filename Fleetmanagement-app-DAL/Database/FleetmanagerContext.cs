using Fleetmanagement_app_Groep1.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fleetmanagement_app_Groep1.Database
{
    public class FleetmanagerContext : DbContext
    {
        public FleetmanagerContext(DbContextOptions<FleetmanagerContext> options) : base(options)
        {
        }

        public FleetmanagerContext(Func<DbContextOptions<FleetmanagerContext>> options)
        {
        }

        public DbSet<Bestuurder> Bestuurders { get; set; }
        public DbSet<Voertuig> Voertuigen { get; set; }
        public DbSet<Tankkaart> Tankkaarten { get; set; }

        public DbSet<Koppeling> Koppelingen { get; set; }

        public DbSet<ToewijzingBrandstofTankkaart> ToewijzingBrandstofTankkaarten { get; set; }
        public DbSet<ToewijzingRijbewijsBestuurder> ToewijzingRijbewijsBestuurders { get; set; }

        public DbSet<Rijbewijs> Rijbewijs { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Brandstof> Brandstof { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Koppeling>(k =>
                {
                    k.ToTable("Koppeling")
                    .HasKey(x => x.KoppelingsId);

                    k.Property(p => p.Rijksregisternummer).IsRequired();
                });

            modelBuilder.Entity<Bestuurder>(b =>
                {
                    b.ToTable("Bestuurder");
                    b.HasKey(k => k.Rijksregisternummer);
                    b.Property(p => p.Naam).IsRequired();
                    b.Property(p => p.Achternaam).IsRequired();
                    b.Property(p => p.GeboorteDatum).IsRequired();
                    b.Property(p => p.Rijksregisternummer).IsRequired().HasMaxLength(11);

                    b.OwnsOne(a => a.Adres);
                    b.HasOne(b => b.Koppeling).WithOne(k => k.Bestuurder).HasForeignKey<Koppeling>(k => k.Rijksregisternummer);
                });

            modelBuilder.Entity<Voertuig>(v =>
                {
                    v.ToTable("Voertuigen");
                    v.HasKey(k => k.Chassisnummer);
                    v.Property(p => p.Merk).IsRequired();
                    v.Property(p => p.Model).IsRequired();
                    v.Property(p => p.BrandstofId).IsRequired();
                    v.Property(p => p.CategorieId).IsRequired();

                    v.HasOne(v => v.Koppeling).WithOne(k => k.Voertuig).HasForeignKey<Koppeling>(f => f.Chassisnummer);
                });

            modelBuilder.Entity<Brandstof>(b =>
                {
                    b.ToTable("Brandstoffen");
                    b.Property(p => p.TypeBrandstof).IsRequired().HasMaxLength(50);
                    b.HasMany(b => b.Voertuigen).WithOne(v => v.Brandstof).HasForeignKey(v => v.BrandstofId);
                });

            modelBuilder.Entity<Tankkaart>(t =>
                {
                    t.ToTable("Tankkaarten");
                    t.HasKey(k => k.Kaartnummer);
                    t.Property(p => p.GeldigheidsDatum).IsRequired().HasColumnType("date");
                    //.HasConversion();
                    t.HasOne(t => t.Koppeling).WithOne(k => k.Tankkaart).HasForeignKey<Koppeling>(k => k.Kaartnummer);
                });

            modelBuilder.Entity<ToewijzingBrandstofTankkaart>(bk =>
                {
                    bk.ToTable("Toewijzingen_Brandstof_Tankkaart");
                    bk.HasKey(t => t.Id);
                    bk.Property(p => p.Tankkaartnummer).IsRequired();
                    bk.Property(p => p.BrandstofId).IsRequired();
                    bk.HasOne(bk => bk.Tankkaart).WithMany(t => t.MogelijkeBrandstoffen).HasForeignKey(fk => fk.Tankkaartnummer);
                    bk.HasOne(bk => bk.Brandstof).WithMany(t => t.Toewijzingen).HasForeignKey(fk => fk.BrandstofId);
                });

            modelBuilder.Entity<ToewijzingRijbewijsBestuurder>(rb =>
               {
                   rb.ToTable("Toewijzingen_Rijbewijs_Bestuurder");
                   rb.HasKey(t => t.Id);
                   rb.Property(p => p.Rijksregisternummer).IsRequired();
                   rb.Property(p => p.RijbewijsId).IsRequired();
                   rb.HasOne(b => b.Bestuurder).WithMany(t => t.ToewijzingenRijbewijs).HasForeignKey(fk => fk.Rijksregisternummer);
                   rb.HasOne(r => r.Rijbewijs).WithMany(t => t.ToewijzingenBestuurder).HasForeignKey(fk => fk.RijbewijsId);
               });

            modelBuilder.Entity<Rijbewijs>().HasData
                (
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "AM" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "A" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "A1" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "A2" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "B M12" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "B" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "B+E" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "C" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "C1" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "C+E" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "C1+E" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "D" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "D1" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "D1+E" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "D+E" },
                new Rijbewijs() { Id = Guid.NewGuid(), TypeRijbewijs = "G" }
                );

            modelBuilder.Entity<Categorie>().HasData
                (
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Hatchback" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Sedan" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Station" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Cabriolet" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Coupé" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "MVP" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "SUV" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "4X4" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Cross-Over" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Dos a dos" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "GT" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Pick- up" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Roadster" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Bestelwagen" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Lichte Vracht" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Vrachtwagen" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "Mini van" },
                    new Categorie() { Id = Guid.NewGuid(), TypeWagen = "bus" }
                );

            modelBuilder.Entity<Brandstof>().HasData
                (
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Benzine" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Diesel" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "euro 98" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "euro 95" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "LPG" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "AdBlue" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Elektrisch" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Hybride Diesel Elektrisch" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Hybride Benzine Elektrisch" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Waterstof" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Biobrandstof" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "Groengas" },
                    new Brandstof() { Id = Guid.NewGuid(), TypeBrandstof = "CNG" }
                );

            modelBuilder.Entity<Status>().HasData
                (
                    new Status() { Id = Guid.NewGuid(), Staat = "aankoop" },
                    new Status() { Id = Guid.NewGuid(), Staat = "garage" },
                    new Status() { Id = Guid.NewGuid(), Staat = "in bedrijf" },
                    new Status() { Id = Guid.NewGuid(), Staat = "uitgeschreven" },
                    new Status() { Id = Guid.NewGuid(), Staat = "onderhoud nodig" }
                );
        }
    }
}