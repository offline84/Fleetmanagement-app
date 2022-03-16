using Fleetmanagement_app_Groep1.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Fleetmanagement_app_Groep1.Database
{
    public class FleetmanagerContext : DbContext
    {
        public FleetmanagerContext(DbContextOptions<FleetmanagerContext> options) : base(options)
        {
        }

        public DbSet<Bestuurder> Bestuurders { get; set; }
        public DbSet<Voertuig> Voertuigen { get; set; }
        //public DbSet<Tankkaart> Tankkaarten {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Koppeling>(k =>
                {
                    k.ToTable("Koppeling")
                    .HasKey(x => x.KoppelingsId);

                    k.Property(p => p.PersoneelsId).IsRequired();
                });

            modelBuilder.Entity<Bestuurder>(b =>
                {
                    b.ToTable("Bestuurder");
                    b.HasKey(k => k.PersoneelsId);
                    b.Property(p => p.Rijksregisternummer).IsRequired().HasMaxLength(11);
                    b.OwnsOne(a => a.Adres);

                    b.HasMany(r => r.Rijbewijzen).WithOne();
                    b.Navigation(b => b.Rijbewijzen).UsePropertyAccessMode(PropertyAccessMode.Property);

                    b.HasOne(b => b.Koppeling).WithOne(k => k.Bestuurder).HasForeignKey<Koppeling>(k => k.PersoneelsId);
                });

            modelBuilder.Entity<Voertuig>(v =>
                {
                    v.ToTable("Voertuigen");
                    v.HasKey(k => k.Chassisnummer);
                    v.Property(p => p.Merk).IsRequired();
                    v.Property(p => p.Model).IsRequired();
                    v.Property(p => p.BrandstofId).IsRequired();
                    v.Property(p => p.AantalDeuren).IsRequired(false);
                    v.Property(p => p.Bouwjaar).IsRequired(false);

                    v.HasOne(v => v.Koppeling).WithOne(k => k.Voertuig).HasForeignKey<Koppeling>(f => f.VoertuigId);
                });

            modelBuilder.Entity<Brandstof>(b =>
                {
                    b.ToTable("Brandstoffen");
                    b.HasKey(k => k.Id);
                    b.Property(p => p.TypeBrandstof).IsRequired().HasMaxLength(50);
                    b.HasMany(b => b.Voertuigen).WithOne(v => v.Brandstof).HasForeignKey(v => v.BrandstofId);
                });

            modelBuilder.Entity<Tankkaart>(t =>
                {
                    t.ToTable("Tankkaarten");
                    t.HasKey(k => k.Kaartnummer);
                    t.Property(p => p.Pincode).IsRequired().HasMaxLength(8);
                    t.Property(p => p.MogelijkeBrandstoffen).IsRequired();
                    t.HasOne(t => t.Koppeling).WithOne(k => k.Tankkaart).HasForeignKey<Koppeling>(k => k.TankkaartId);
                });

            modelBuilder.Entity<ToewijzingBrandstofTankkaart>(bk => 
                {
                    bk.ToTable("Toewijzing_Brandstof_Tankkaart");
                    bk.HasKey(t => t.Id);
                    bk.Property(p => p.TankkaartId).IsRequired();
                    bk.Property(p => p.BrandstofId).IsRequired();
                    bk.HasOne(bk => bk.Tankkaart).WithMany(t => t.MogelijkeBrandstoffen).HasForeignKey(fk => fk.TankkaartId);
                    bk.HasOne(bk => bk.Brandstof).WithMany(t => t.Toewijzingen).HasForeignKey(fk => fk.BrandstofId);

                });
        }
    }
}