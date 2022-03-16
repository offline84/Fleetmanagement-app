using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fleetmanagement_app_Groep1.Migrations
{
    public partial class VoertuigMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoertuigId",
                table: "Koppeling",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Brandstof",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeBrandstof = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brandstof", x => x.Id);
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
                        name: "FK_Voertuigen_Brandstof_BrandstofId",
                        column: x => x.BrandstofId,
                        principalTable: "Brandstof",
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

            migrationBuilder.CreateIndex(
                name: "IX_Koppeling_VoertuigId",
                table: "Koppeling",
                column: "VoertuigId",
                unique: true,
                filter: "[VoertuigId] IS NOT NULL");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Koppeling_Voertuigen_VoertuigId",
                table: "Koppeling",
                column: "VoertuigId",
                principalTable: "Voertuigen",
                principalColumn: "Chassisnummer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Koppeling_Voertuigen_VoertuigId",
                table: "Koppeling");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "Brandstof");

            migrationBuilder.DropTable(
                name: "Categorie");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Koppeling_VoertuigId",
                table: "Koppeling");

            migrationBuilder.DropColumn(
                name: "VoertuigId",
                table: "Koppeling");
        }
    }
}
