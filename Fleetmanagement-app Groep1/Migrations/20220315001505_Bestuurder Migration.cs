using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fleetmanagement_app_Groep1.Migrations
{
    public partial class BestuurderMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bestuurder",
                columns: table => new
                {
                    PersoneelsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Achternaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Adres_Straat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres_Huisnummer = table.Column<int>(type: "int", nullable: true),
                    Adres_Stad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres_Postcode = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    GeboorteDatum = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    Rijksregisternummer = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsGearchiveerd = table.Column<bool>(type: "bit", nullable: false),
                    LaatstGeupdate = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestuurder", x => x.PersoneelsId);
                });

            migrationBuilder.CreateTable(
                name: "Koppeling",
                columns: table => new
                {
                    KoppelingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersoneelsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koppeling", x => x.KoppelingsId);
                    table.ForeignKey(
                        name: "FK_Koppeling_Bestuurder_PersoneelsId",
                        column: x => x.PersoneelsId,
                        principalTable: "Bestuurder",
                        principalColumn: "PersoneelsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rijbewijs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeRijbewijs = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BestuurderPersoneelsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rijbewijs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rijbewijs_Bestuurder_BestuurderPersoneelsId",
                        column: x => x.BestuurderPersoneelsId,
                        principalTable: "Bestuurder",
                        principalColumn: "PersoneelsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Koppeling_PersoneelsId",
                table: "Koppeling",
                column: "PersoneelsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rijbewijs_BestuurderPersoneelsId",
                table: "Rijbewijs",
                column: "BestuurderPersoneelsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Koppeling");

            migrationBuilder.DropTable(
                name: "Rijbewijs");

            migrationBuilder.DropTable(
                name: "Bestuurder");
        }
    }
}
