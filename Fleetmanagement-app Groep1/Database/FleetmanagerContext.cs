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

        public DbSet<Bestuurder> Bestuurders { get; set; }
        public DbSet<Voertuig> Voertuigen { get; set; }
        public DbSet<Tankkaart> Tankkaarten { get; set; }

        public DbSet<Koppeling> Koppelingen {get; set;}

        public DbSet<ToewijzingBrandstofTankkaart> ToewijzingBrandstofTankkaarten {get; set;}
        public DbSet<ToewijzingRijbewijsBestuurder> ToewijzingRijbewijsBestuurders {get; set;}

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

            //modelBuilder.Entity<Rijbewijs>().HasData
            //    (
            //    new Rijbewijs(){TypeRijbewijs = "AM"},
            //    new Rijbewijs(){TypeRijbewijs = "A"},
            //    new Rijbewijs(){TypeRijbewijs = "A1"},
            //    new Rijbewijs(){TypeRijbewijs = "A2"},
            //    new Rijbewijs(){TypeRijbewijs = "B M12"},
            //    new Rijbewijs(){TypeRijbewijs = "B"},
            //    new Rijbewijs(){TypeRijbewijs = "B+E"},
            //    new Rijbewijs(){TypeRijbewijs = "C"},
            //    new Rijbewijs(){TypeRijbewijs = "C1"},
            //    new Rijbewijs(){TypeRijbewijs = "C+E"},
            //    new Rijbewijs(){TypeRijbewijs = "C1+E"},
            //    new Rijbewijs(){TypeRijbewijs = "D"},
            //    new Rijbewijs(){TypeRijbewijs = "D1"},
            //    new Rijbewijs(){TypeRijbewijs = "D1+E"},
            //    new Rijbewijs(){TypeRijbewijs = "D+E"},
            //    new Rijbewijs(){TypeRijbewijs = "G"}
            //    );

            //modelBuilder.Entity<Categorie>().HasData
            //    (
            //        new Categorie(){TypeWagen = "Hatchback"},
            //        new Categorie(){TypeWagen = "Sedan"},
            //        new Categorie(){TypeWagen = "Station"},
            //        new Categorie(){TypeWagen = "Cabriolet"},
            //        new Categorie(){TypeWagen = "Coupé"},
            //        new Categorie(){TypeWagen = "MVP"},
            //        new Categorie(){TypeWagen = "SUV"},
            //        new Categorie(){TypeWagen = "4X4"},
            //        new Categorie(){TypeWagen = "Cross-Over"},
            //        new Categorie(){TypeWagen = "Dos a dos"},
            //        new Categorie(){TypeWagen = "GT"},
            //        new Categorie(){TypeWagen = "Pick- up"},
            //        new Categorie(){TypeWagen = "Roadster"},
            //        new Categorie(){TypeWagen = "Bestelwagen"},
            //        new Categorie(){TypeWagen = "Lichte Vracht"},
            //        new Categorie(){TypeWagen = "Vrachtwagen"},
            //        new Categorie(){TypeWagen = "Mini van"},
            //        new Categorie(){TypeWagen = "bus"}
            //    );

            //modelBuilder.Entity<Brandstof>().HasData
            //    (
            //        new Brandstof(){Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), TypeBrandstof = "Benzine"},
            //        new Brandstof(){Id = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), TypeBrandstof = "Diesel"},
            //        new Brandstof(){Id = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"), TypeBrandstof = "euro 98"},
            //        new Brandstof(){Id = Guid.Parse("102b566b-ba1f-404c-b2df-e2cde39ade09"), TypeBrandstof = "euro 95"},
            //        new Brandstof(){Id = Guid.Parse("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"), TypeBrandstof = "LPG"},
            //        new Brandstof(){Id = Guid.Parse("2aadd2df-7caf-45ab-9355-7f6332985a87"), TypeBrandstof = "AdBlue"},
            //        new Brandstof(){Id = Guid.Parse("2ee49fe3-edf2-4f91-8409-3eb25ce6ca51"), TypeBrandstof = "Elektrisch"},
            //        new Brandstof(){Id = Guid.Parse("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"), TypeBrandstof = "Hybride Diesel Elektrisch"},
            //        new Brandstof(){Id = Guid.Parse("d8663e5e-7494-4f81-8739-6e0de1bea7ee"), TypeBrandstof = "Hybride Benzine Elektrisch"},
            //        new Brandstof(){Id = Guid.Parse("d173e20d-159e-4127-9ce9-b0ac2564ad97"), TypeBrandstof = "Waterstof"},
            //        new Brandstof(){Id = Guid.Parse("40ff5488-fdab-45b5-bc3a-14302d59869a"), TypeBrandstof = "Biobrandstof"},
            //        new Brandstof(){Id = Guid.Parse("2baab6b6-a44a-4c34-9232-e02966ca7e80"), TypeBrandstof = "Groengas"},
            //        new Brandstof(){Id = Guid.Parse("50b106ca-f711-4b6f-9405-8c8b8fdce7c2"), TypeBrandstof = "CNG"}
            //    );

            //modelBuilder.Entity<Status>().HasData
            //    (
            //        new Status(){Staat = "aankoop"},
            //        new Status(){Staat = "garage"},
            //        new Status(){Staat = "in bedrijf"},
            //        new Status(){Staat = "uitgeschreven"},
            //        new Status(){Staat = "onderhoud nodig"}
            //    );
        }
    }
}