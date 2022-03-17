using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fleetmanagement_app_Groep1.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bestuurder",
                columns: table => new
                {
                    Rijksregisternummer = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Achternaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adres_Straat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres_Huisnummer = table.Column<int>(type: "int", nullable: true),
                    Adres_Stad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres_Postcode = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    IsGearchiveerd = table.Column<bool>(type: "bit", nullable: false),
                    LaatstGeupdate = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestuurder", x => x.Rijksregisternummer);
                });

            migrationBuilder.CreateTable(
                name: "Brandstoffen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeBrandstof = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brandstoffen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeWagen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rijbewijs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeRijbewijs = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rijbewijs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Staat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tankkaarten",
                columns: table => new
                {
                    Kaartnummer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeldigheidsDatum = table.Column<DateTime>(type: "date", rowVersion: true, nullable: false),
                    Pincode = table.Column<int>(type: "int", maxLength: 8, nullable: false),
                    IsGeblokkeerd = table.Column<bool>(type: "bit", nullable: false),
                    IsGearchiveerd = table.Column<bool>(type: "bit", nullable: false),
                    LaatstGeupdate = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tankkaarten", x => x.Kaartnummer);
                });

            migrationBuilder.CreateTable(
                name: "Toewijzingen_Rijbewijs_Bestuurder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rijksregisternummer = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    RijbewijsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toewijzingen_Rijbewijs_Bestuurder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toewijzingen_Rijbewijs_Bestuurder_Bestuurder_Rijksregisternummer",
                        column: x => x.Rijksregisternummer,
                        principalTable: "Bestuurder",
                        principalColumn: "Rijksregisternummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Toewijzingen_Rijbewijs_Bestuurder_Rijbewijs_RijbewijsId",
                        column: x => x.RijbewijsId,
                        principalTable: "Rijbewijs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voertuigen",
                columns: table => new
                {
                    Chassisnummer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Merk = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bouwjaar = table.Column<int>(type: "int", nullable: false),
                    Nummerplaat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BrandstofId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategorieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kleur = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AantalDeuren = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsGearchiveerd = table.Column<bool>(type: "bit", nullable: false),
                    LaatstGeupdate = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.Chassisnummer);
                    table.ForeignKey(
                        name: "FK_Voertuigen_Brandstoffen_BrandstofId",
                        column: x => x.BrandstofId,
                        principalTable: "Brandstoffen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voertuigen_Categorie_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voertuigen_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Toewijzingen_Brandstof_Tankkaart",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tankkaartnummer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BrandstofId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toewijzingen_Brandstof_Tankkaart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toewijzingen_Brandstof_Tankkaart_Brandstoffen_BrandstofId",
                        column: x => x.BrandstofId,
                        principalTable: "Brandstoffen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Toewijzingen_Brandstof_Tankkaart_Tankkaarten_Tankkaartnummer",
                        column: x => x.Tankkaartnummer,
                        principalTable: "Tankkaarten",
                        principalColumn: "Kaartnummer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Koppeling",
                columns: table => new
                {
                    KoppelingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rijksregisternummer = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    Chassisnummer = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Kaartnummer = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koppeling", x => x.KoppelingsId);
                    table.ForeignKey(
                        name: "FK_Koppeling_Bestuurder_Rijksregisternummer",
                        column: x => x.Rijksregisternummer,
                        principalTable: "Bestuurder",
                        principalColumn: "Rijksregisternummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Koppeling_Tankkaarten_Kaartnummer",
                        column: x => x.Kaartnummer,
                        principalTable: "Tankkaarten",
                        principalColumn: "Kaartnummer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Koppeling_Voertuigen_Chassisnummer",
                        column: x => x.Chassisnummer,
                        principalTable: "Voertuigen",
                        principalColumn: "Chassisnummer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Koppeling_Chassisnummer",
                table: "Koppeling",
                column: "Chassisnummer",
                unique: true,
                filter: "[Chassisnummer] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Koppeling_Kaartnummer",
                table: "Koppeling",
                column: "Kaartnummer",
                unique: true,
                filter: "[Kaartnummer] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Koppeling_Rijksregisternummer",
                table: "Koppeling",
                column: "Rijksregisternummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Toewijzingen_Brandstof_Tankkaart_BrandstofId",
                table: "Toewijzingen_Brandstof_Tankkaart",
                column: "BrandstofId");

            migrationBuilder.CreateIndex(
                name: "IX_Toewijzingen_Brandstof_Tankkaart_Tankkaartnummer",
                table: "Toewijzingen_Brandstof_Tankkaart",
                column: "Tankkaartnummer");

            migrationBuilder.CreateIndex(
                name: "IX_Toewijzingen_Rijbewijs_Bestuurder_RijbewijsId",
                table: "Toewijzingen_Rijbewijs_Bestuurder",
                column: "RijbewijsId");

            migrationBuilder.CreateIndex(
                name: "IX_Toewijzingen_Rijbewijs_Bestuurder_Rijksregisternummer",
                table: "Toewijzingen_Rijbewijs_Bestuurder",
                column: "Rijksregisternummer");

            migrationBuilder.CreateIndex(
                name: "IX_Voertuigen_BrandstofId",
                table: "Voertuigen",
                column: "BrandstofId");

            migrationBuilder.CreateIndex(
                name: "IX_Voertuigen_CategorieId",
                table: "Voertuigen",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_Voertuigen_StatusId",
                table: "Voertuigen",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Koppeling");

            migrationBuilder.DropTable(
                name: "Toewijzingen_Brandstof_Tankkaart");

            migrationBuilder.DropTable(
                name: "Toewijzingen_Rijbewijs_Bestuurder");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "Tankkaarten");

            migrationBuilder.DropTable(
                name: "Bestuurder");

            migrationBuilder.DropTable(
                name: "Rijbewijs");

            migrationBuilder.DropTable(
                name: "Brandstoffen");

            migrationBuilder.DropTable(
                name: "Categorie");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
