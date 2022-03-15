using Microsoft.EntityFrameworkCore;
using Fleetmanagement_app_Groep1.Entities;
using Microsoft.Extensions.Configuration;

namespace Fleetmanagement_app_Groep1.Database
{
    public class FleetmanagerContext : DbContext
    {
       
        public FleetmanagerContext(DbContextOptions<FleetmanagerContext> options ) : base(options)
        {

        }

        public DbSet<Bestuurder> Bestuurders {get; set;}
        //public DbSet<Voertuig> Voertuigen {get; set;}
        //public DbSet<Tankkaart> Tankkaarten {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            
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
                
            modelBuilder.Entity<Koppeling>(k =>
                {
                    k.ToTable("Koppeling")
                    .HasKey(x => x.KoppelingsId);
                    
                    k.Property(p => p.PersoneelsId).IsRequired();
                    
                });
            
        }

    }
}