﻿using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
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
        public DbSet<Voertuig> Voertuigen {get; set;}
        public DbSet<Tankkaart> Tankkaarten {get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        //modelBuilder.Entity<Bestuurder>()
 
        //    .HasKey(b => b.PersoneelsId)
        //    .HasOne(b => b.Tankkaart)
        //    .WithOne(t=> t.Bestuurder)
        //    .HasForeignKey<Bestuurder>(t => t.TankkaartId)
        //    .HasPrincipalKey<Tankkaart>(t => t.Kaartnummer);
        modelBuilder.CreateTable(
                name: "Bestuurders",
                columns: table => new
                {
                    PersoneelsId = table.Column<string>(nullable: false),
                    Naam = table.Column<string>(nullable: false, maxLength: 50),
                    Achternaam = table.Column<string>(nullable: false, maxLength: 50),
                    Straat = table.Column<string>(nullable: false, maxLength: 100),
                    Huisnummer = table.Column<int>(nullable: false, maxLength: 50),
                    Stad = table.Column<string>(nullable: false, maxLength: 50),
                    Postcode = table.Column<int>(nullable: false, maxLength: 50),
                    GeboorteDatum = table.Column<DateTime>(nullable: false),
                    RijksregisterNummer = table.Column<string>(nullable: false, maxLength: 11),
                    VoertuigId = table.Column<string>(nullable: false, maxLength: 50),
                    TankkaartId = table.Column<string>(nullable: false, maxLength: 50),
                    IsGearchiveerd = table.Column<bool>(defaultValue: false),
                    LaatstGeupdate = table.Column<DateTime>(nullable: true, defaultValue: null)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestuurders", x => x.PersoneelsId);
                    table.ForeignKey(
                        name: "FK_Bestuurders_Voertuigen_VoertuigId",
                        column: x => x.VoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "Chassisnummer",
                        onDelete: ReferentialAction.NoAction
                        );
                    table.ForeignKey(
                        name: "FK_Bestuurders_Tankkaarten_TankkaartId",
                        column: x => x.TankkaartId,
                        principalTable: "Tankkaarten",
                        principalColumn: "Kaartnummer",
                        onDelete: ReferentialAction.NoAction
                        );
                });

            migrationBuilder.CreateTable(
                name: "Tankkaarten",
                columns: table => new
                {
                    kaartnummer = table.Column<string>(nullable: false),
                    GeldigheidsDatum = table.Column<DateTime>(nullable: false),
                    Pincode = table.Column<int>(nullable: false, maxLength: 6),
                    BestuurdersId = table.Column<string>(nullable: true),
                    IsGeblokkeerd = table.Column<bool>(nullable: false, defaultValue: false),
                    IsGearchiveerd = table.Column<bool>(defaultValue: false),
                    LaatstGeupdate = table.Column<DateTime>(nullable: true, defaultValue: null)
                },
                constraints: table =>
               {
                   table.PrimaryKey("PK_Tankkaarten", x => x.kaartnummer);
                   table.ForeignKey(
                       name: "FK_Tankkaarten_Bestuurders_BestuurdersId",
                       column: x => x.BestuurdersId,
                       principalTable: "Bestuurders",
                       principalColumn: "PersoneelsId",
                       onDelete: ReferentialAction.NoAction
                       );
               });
        
        }

    }
}