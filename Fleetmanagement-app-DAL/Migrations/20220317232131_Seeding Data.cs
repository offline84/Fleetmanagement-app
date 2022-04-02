using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fleetmanagement_app_Groep1.Migrations
{
    public partial class SeedingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brandstoffen",
                columns: new[] { "Id", "TypeBrandstof" },
                values: new object[,]
                {
                    { new Guid("35fc59f4-c432-4b21-8164-c89b69a7eebe"), "Benzine" },
                    { new Guid("c987352a-6ad1-46b0-823e-8ae59e856af6"), "CNG" },
                    { new Guid("fc9256cd-7963-4339-83f4-32fed199348b"), "Groengas" },
                    { new Guid("b38f7ca8-7d4a-4d85-b26c-edbc23594660"), "Waterstof" },
                    { new Guid("aceded2b-4cef-42ca-8cb6-8ac3b8e84a77"), "Hybride Benzine Elektrisch" },
                    { new Guid("5b23758f-49ab-4611-bf51-a0acb3e7e980"), "Hybride Diesel Elektrisch" },
                    { new Guid("d05e108e-a969-4db0-989e-f28a5171741c"), "Biobrandstof" },
                    { new Guid("398568e9-9588-474d-9bfc-4eebf809ec1e"), "AdBlue" },
                    { new Guid("abb84655-4470-44a4-9b6f-53e935cb1f1b"), "LPG" },
                    { new Guid("c8b4f4d0-e7aa-46b7-847e-44ab85e36a01"), "euro 95" },
                    { new Guid("adf36a35-28ce-4840-afd8-bba072c64a46"), "euro 98" },
                    { new Guid("97dc688e-5200-427d-8442-66a6a867facf"), "Diesel" },
                    { new Guid("ab02814b-7170-4d90-9393-570d767cb8f4"), "Elektrisch" }
                });

            migrationBuilder.InsertData(
                table: "Categorie",
                columns: new[] { "Id", "TypeWagen" },
                values: new object[,]
                {
                    { new Guid("4391da67-bbfd-4240-b111-c0f6c2e6d3df"), "GT" },
                    { new Guid("abaa10d1-378d-45a0-9409-e7f9b809c133"), "bus" },
                    { new Guid("4d5eb36d-1724-4b97-8717-6601ba07b028"), "Mini van" },
                    { new Guid("04275258-47cf-4b64-82b9-e76e366038ce"), "Vrachtwagen" },
                    { new Guid("f79ea01e-a4ec-49ee-ae63-7b7a65e5a33f"), "Lichte Vracht" },
                    { new Guid("be6b1b38-d1c1-48f8-b45b-afe6e6b1ef67"), "Bestelwagen" },
                    { new Guid("271bad9a-1a2c-4a45-b9dd-f5d0afb9198a"), "Pick- up" },
                    { new Guid("c36d10e1-638a-457f-8df2-16a2ebc8c53c"), "Dos a dos" },
                    { new Guid("fbe007b5-1fb4-4a44-bd6c-0833ee0c0179"), "Roadster" },
                    { new Guid("73425593-bc42-4cdc-b116-a42d7a72211f"), "4X4" },
                    { new Guid("261900a7-3397-4b6a-847a-70c8a1eea7f4"), "Cross-Over" },
                    { new Guid("89a20ea2-b748-4a26-926a-6e73533fa5e0"), "Sedan" },
                    { new Guid("44e5092f-514a-410b-a3c3-6622c3c01d61"), "Station" },
                    { new Guid("ebc50d3b-46f8-401a-ae23-9cbfe38ec290"), "Cabriolet" },
                    { new Guid("b2a274db-3e19-484b-8790-682e4578aeff"), "Hatchback" },
                    { new Guid("9d5dc338-1cf8-47d5-ad32-2543309c58e4"), "MVP" },
                    { new Guid("94436710-40d6-466f-8ff9-1180f9801db1"), "SUV" },
                    { new Guid("e8e87b0d-9f9a-4d0e-99aa-11d1e3a01187"), "Coupé" }
                });

            migrationBuilder.InsertData(
                table: "Rijbewijs",
                columns: new[] { "Id", "TypeRijbewijs" },
                values: new object[,]
                {
                    { new Guid("3ae0d296-1225-4949-aa12-0e73f11b1995"), "G" },
                    { new Guid("a374832a-d2fd-4d02-acd8-671788e76004"), "C+E" },
                    { new Guid("b693ff25-1d3c-48d5-8f64-e5a1818991c6"), "D+E" },
                    { new Guid("dbddbe14-6842-44ed-a6a2-75d1fb23af4b"), "D1+E" },
                    { new Guid("b9d5428e-242e-4b66-ac2e-9e189747cc79"), "D1" },
                    { new Guid("1247f201-ca7f-4d5f-83dd-ef8aa351ea8d"), "D" },
                    { new Guid("39e8718b-e6b0-45d3-8dc3-675577051835"), "C1+E" },
                    { new Guid("847721ed-eb8d-4621-9040-4c84600876b0"), "C1" },
                    { new Guid("8b4240b1-c0e0-4956-b262-8ced1133d330"), "B" },
                    { new Guid("dd0b0b82-9e82-4d28-b44b-9cfe176c2907"), "B+E" },
                    { new Guid("91ed00a3-f3eb-4993-b454-74ce825f95c3"), "B M12" },
                    { new Guid("de85737f-f913-43d0-bbf4-86360cf98285"), "A2" },
                    { new Guid("a53a3bfc-ba64-48e4-b96b-7ce2d91c42fe"), "A1" },
                    { new Guid("6e99c209-ef31-4171-ba31-3e0eafe06d84"), "A" },
                    { new Guid("b72f62eb-7d82-4155-a44e-39fda40b4480"), "AM" },
                    { new Guid("d0672e9d-994a-49f2-8e98-72deaa83262b"), "C" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "Staat" },
                values: new object[,]
                {
                    { new Guid("850c6b56-183b-4cff-bb56-aaf267cada9d"), "uitgeschreven" },
                    { new Guid("f04d946a-84d4-43b0-b730-590d25a8a913"), "aankoop" },
                    { new Guid("259f5c51-49b7-4212-b779-29835522172b"), "garage" },
                    { new Guid("124a3594-36d8-41f3-a270-15b78808a46a"), "in bedrijf" },
                    { new Guid("8a421e86-0b31-43ec-99e1-346432f3053a"), "onderhoud nodig" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("35fc59f4-c432-4b21-8164-c89b69a7eebe"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("398568e9-9588-474d-9bfc-4eebf809ec1e"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("5b23758f-49ab-4611-bf51-a0acb3e7e980"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("97dc688e-5200-427d-8442-66a6a867facf"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("ab02814b-7170-4d90-9393-570d767cb8f4"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("abb84655-4470-44a4-9b6f-53e935cb1f1b"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("aceded2b-4cef-42ca-8cb6-8ac3b8e84a77"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("adf36a35-28ce-4840-afd8-bba072c64a46"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("b38f7ca8-7d4a-4d85-b26c-edbc23594660"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("c8b4f4d0-e7aa-46b7-847e-44ab85e36a01"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("c987352a-6ad1-46b0-823e-8ae59e856af6"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("d05e108e-a969-4db0-989e-f28a5171741c"));

            migrationBuilder.DeleteData(
                table: "Brandstoffen",
                keyColumn: "Id",
                keyValue: new Guid("fc9256cd-7963-4339-83f4-32fed199348b"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("04275258-47cf-4b64-82b9-e76e366038ce"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("261900a7-3397-4b6a-847a-70c8a1eea7f4"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("271bad9a-1a2c-4a45-b9dd-f5d0afb9198a"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("4391da67-bbfd-4240-b111-c0f6c2e6d3df"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("44e5092f-514a-410b-a3c3-6622c3c01d61"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("4d5eb36d-1724-4b97-8717-6601ba07b028"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("73425593-bc42-4cdc-b116-a42d7a72211f"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("89a20ea2-b748-4a26-926a-6e73533fa5e0"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("94436710-40d6-466f-8ff9-1180f9801db1"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("9d5dc338-1cf8-47d5-ad32-2543309c58e4"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("abaa10d1-378d-45a0-9409-e7f9b809c133"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("b2a274db-3e19-484b-8790-682e4578aeff"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("be6b1b38-d1c1-48f8-b45b-afe6e6b1ef67"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("c36d10e1-638a-457f-8df2-16a2ebc8c53c"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("e8e87b0d-9f9a-4d0e-99aa-11d1e3a01187"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("ebc50d3b-46f8-401a-ae23-9cbfe38ec290"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("f79ea01e-a4ec-49ee-ae63-7b7a65e5a33f"));

            migrationBuilder.DeleteData(
                table: "Categorie",
                keyColumn: "Id",
                keyValue: new Guid("fbe007b5-1fb4-4a44-bd6c-0833ee0c0179"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("1247f201-ca7f-4d5f-83dd-ef8aa351ea8d"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("39e8718b-e6b0-45d3-8dc3-675577051835"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("3ae0d296-1225-4949-aa12-0e73f11b1995"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("6e99c209-ef31-4171-ba31-3e0eafe06d84"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("847721ed-eb8d-4621-9040-4c84600876b0"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("8b4240b1-c0e0-4956-b262-8ced1133d330"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("91ed00a3-f3eb-4993-b454-74ce825f95c3"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("a374832a-d2fd-4d02-acd8-671788e76004"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("a53a3bfc-ba64-48e4-b96b-7ce2d91c42fe"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("b693ff25-1d3c-48d5-8f64-e5a1818991c6"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("b72f62eb-7d82-4155-a44e-39fda40b4480"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("b9d5428e-242e-4b66-ac2e-9e189747cc79"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("d0672e9d-994a-49f2-8e98-72deaa83262b"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("dbddbe14-6842-44ed-a6a2-75d1fb23af4b"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("dd0b0b82-9e82-4d28-b44b-9cfe176c2907"));

            migrationBuilder.DeleteData(
                table: "Rijbewijs",
                keyColumn: "Id",
                keyValue: new Guid("de85737f-f913-43d0-bbf4-86360cf98285"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("124a3594-36d8-41f3-a270-15b78808a46a"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("259f5c51-49b7-4212-b779-29835522172b"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("850c6b56-183b-4cff-bb56-aaf267cada9d"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("8a421e86-0b31-43ec-99e1-346432f3053a"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("f04d946a-84d4-43b0-b730-590d25a8a913"));
        }
    }
}