﻿// <auto-generated />
using System;
using Fleetmanagement_app_DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fleetmanagement_app_DAL.Migrations
{
    [DbContext(typeof(FleetmanagerContext))]
    partial class FleetmanagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Bestuurder", b =>
                {
                    b.Property<string>("Rijksregisternummer")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Achternaam")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("GeboorteDatum")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsGearchiveerd")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LaatstGeupdate")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Naam")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Rijksregisternummer");

                    b.ToTable("Bestuurder");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Brandstof", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TypeBrandstof")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Brandstoffen");


                    b.HasData(
                        new
                        {
                            Id = new Guid("35fc59f4-c432-4b21-8164-c89b69a7eebe"),
                            TypeBrandstof = "Benzine"
                        },
                        new
                        {
                            Id = new Guid("97dc688e-5200-427d-8442-66a6a867facf"),
                            TypeBrandstof = "Diesel"
                        },
                        new
                        {
                            Id = new Guid("adf36a35-28ce-4840-afd8-bba072c64a46"),
                            TypeBrandstof = "euro 98"
                        },
                        new
                        {
                            Id = new Guid("c8b4f4d0-e7aa-46b7-847e-44ab85e36a01"),
                            TypeBrandstof = "euro 95"
                        },
                        new
                        {
                            Id = new Guid("abb84655-4470-44a4-9b6f-53e935cb1f1b"),
                            TypeBrandstof = "LPG"
                        },
                        new
                        {
                            Id = new Guid("398568e9-9588-474d-9bfc-4eebf809ec1e"),
                            TypeBrandstof = "AdBlue"
                        },
                        new
                        {
                            Id = new Guid("ab02814b-7170-4d90-9393-570d767cb8f4"),
                            TypeBrandstof = "Elektrisch"
                        },
                        new
                        {
                            Id = new Guid("5b23758f-49ab-4611-bf51-a0acb3e7e980"),
                            TypeBrandstof = "Hybride Diesel Elektrisch"
                        },
                        new
                        {
                            Id = new Guid("aceded2b-4cef-42ca-8cb6-8ac3b8e84a77"),
                            TypeBrandstof = "Hybride Benzine Elektrisch"
                        },
                        new
                        {
                            Id = new Guid("b38f7ca8-7d4a-4d85-b26c-edbc23594660"),
                            TypeBrandstof = "Waterstof"
                        },
                        new
                        {
                            Id = new Guid("d05e108e-a969-4db0-989e-f28a5171741c"),
                            TypeBrandstof = "Biobrandstof"
                        },
                        new
                        {
                            Id = new Guid("fc9256cd-7963-4339-83f4-32fed199348b"),
                            TypeBrandstof = "Groengas"
                        },
                        new
                        {
                            Id = new Guid("c987352a-6ad1-46b0-823e-8ae59e856af6"),
                            TypeBrandstof = "CNG"
                        });
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Categorie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TypeWagen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categorie");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b2a274db-3e19-484b-8790-682e4578aeff"),
                            TypeWagen = "Hatchback"
                        },
                        new
                        {
                            Id = new Guid("89a20ea2-b748-4a26-926a-6e73533fa5e0"),
                            TypeWagen = "Sedan"
                        },
                        new
                        {
                            Id = new Guid("44e5092f-514a-410b-a3c3-6622c3c01d61"),
                            TypeWagen = "Station"
                        },
                        new
                        {
                            Id = new Guid("ebc50d3b-46f8-401a-ae23-9cbfe38ec290"),
                            TypeWagen = "Cabriolet"
                        },
                        new
                        {
                            Id = new Guid("e8e87b0d-9f9a-4d0e-99aa-11d1e3a01187"),
                            TypeWagen = "Coupé"
                        },
                        new
                        {
                            Id = new Guid("9d5dc338-1cf8-47d5-ad32-2543309c58e4"),
                            TypeWagen = "MVP"
                        },
                        new
                        {
                            Id = new Guid("94436710-40d6-466f-8ff9-1180f9801db1"),
                            TypeWagen = "SUV"
                        },
                        new
                        {
                            Id = new Guid("73425593-bc42-4cdc-b116-a42d7a72211f"),
                            TypeWagen = "4X4"
                        },
                        new
                        {
                            Id = new Guid("261900a7-3397-4b6a-847a-70c8a1eea7f4"),
                            TypeWagen = "Cross-Over"
                        },
                        new
                        {
                            Id = new Guid("c36d10e1-638a-457f-8df2-16a2ebc8c53c"),
                            TypeWagen = "Dos a dos"
                        },
                        new
                        {
                            Id = new Guid("4391da67-bbfd-4240-b111-c0f6c2e6d3df"),
                            TypeWagen = "GT"
                        },
                        new
                        {
                            Id = new Guid("271bad9a-1a2c-4a45-b9dd-f5d0afb9198a"),
                            TypeWagen = "Pick- up"
                        },
                        new
                        {
                            Id = new Guid("fbe007b5-1fb4-4a44-bd6c-0833ee0c0179"),
                            TypeWagen = "Roadster"
                        },
                        new
                        {
                            Id = new Guid("be6b1b38-d1c1-48f8-b45b-afe6e6b1ef67"),
                            TypeWagen = "Bestelwagen"
                        },
                        new
                        {
                            Id = new Guid("f79ea01e-a4ec-49ee-ae63-7b7a65e5a33f"),
                            TypeWagen = "Lichte Vracht"
                        },
                        new
                        {
                            Id = new Guid("04275258-47cf-4b64-82b9-e76e366038ce"),
                            TypeWagen = "Vrachtwagen"
                        },
                        new
                        {
                            Id = new Guid("4d5eb36d-1724-4b97-8717-6601ba07b028"),
                            TypeWagen = "Mini van"
                        },
                        new
                        {
                            Id = new Guid("abaa10d1-378d-45a0-9409-e7f9b809c133"),
                            TypeWagen = "bus"
                        });
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Koppeling", b =>
                {
                    b.Property<Guid>("KoppelingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Chassisnummer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Kaartnummer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Rijksregisternummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("KoppelingsId");

                    b.HasIndex("Chassisnummer")
                        .IsUnique()
                        .HasFilter("[Chassisnummer] IS NOT NULL");

                    b.HasIndex("Kaartnummer")
                        .IsUnique()
                        .HasFilter("[Kaartnummer] IS NOT NULL");

                    b.HasIndex("Rijksregisternummer")
                        .IsUnique();

                    b.ToTable("Koppeling");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Rijbewijs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TypeRijbewijs")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Rijbewijs");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b72f62eb-7d82-4155-a44e-39fda40b4480"),
                            TypeRijbewijs = "AM"
                        },
                        new
                        {
                            Id = new Guid("6e99c209-ef31-4171-ba31-3e0eafe06d84"),
                            TypeRijbewijs = "A"
                        },
                        new
                        {
                            Id = new Guid("a53a3bfc-ba64-48e4-b96b-7ce2d91c42fe"),
                            TypeRijbewijs = "A1"
                        },
                        new
                        {
                            Id = new Guid("de85737f-f913-43d0-bbf4-86360cf98285"),
                            TypeRijbewijs = "A2"
                        },
                        new
                        {
                            Id = new Guid("91ed00a3-f3eb-4993-b454-74ce825f95c3"),
                            TypeRijbewijs = "B M12"
                        },
                        new
                        {
                            Id = new Guid("8b4240b1-c0e0-4956-b262-8ced1133d330"),
                            TypeRijbewijs = "B"
                        },
                        new
                        {
                            Id = new Guid("dd0b0b82-9e82-4d28-b44b-9cfe176c2907"),
                            TypeRijbewijs = "B+E"
                        },
                        new
                        {
                            Id = new Guid("d0672e9d-994a-49f2-8e98-72deaa83262b"),
                            TypeRijbewijs = "C"
                        },
                        new
                        {
                            Id = new Guid("847721ed-eb8d-4621-9040-4c84600876b0"),
                            TypeRijbewijs = "C1"
                        },
                        new
                        {
                            Id = new Guid("a374832a-d2fd-4d02-acd8-671788e76004"),
                            TypeRijbewijs = "C+E"
                        },
                        new
                        {
                            Id = new Guid("39e8718b-e6b0-45d3-8dc3-675577051835"),
                            TypeRijbewijs = "C1+E"
                        },
                        new
                        {
                            Id = new Guid("1247f201-ca7f-4d5f-83dd-ef8aa351ea8d"),
                            TypeRijbewijs = "D"
                        },
                        new
                        {
                            Id = new Guid("b9d5428e-242e-4b66-ac2e-9e189747cc79"),
                            TypeRijbewijs = "D1"
                        },
                        new
                        {
                            Id = new Guid("dbddbe14-6842-44ed-a6a2-75d1fb23af4b"),
                            TypeRijbewijs = "D1+E"
                        },
                        new
                        {
                            Id = new Guid("b693ff25-1d3c-48d5-8f64-e5a1818991c6"),
                            TypeRijbewijs = "D+E"
                        },
                        new
                        {
                            Id = new Guid("3ae0d296-1225-4949-aa12-0e73f11b1995"),
                            TypeRijbewijs = "G"
                        });
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Status", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Staat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Status");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f04d946a-84d4-43b0-b730-590d25a8a913"),
                            Staat = "aankoop"
                        },
                        new
                        {
                            Id = new Guid("259f5c51-49b7-4212-b779-29835522172b"),
                            Staat = "garage"
                        },
                        new
                        {
                            Id = new Guid("124a3594-36d8-41f3-a270-15b78808a46a"),
                            Staat = "in bedrijf"
                        },
                        new
                        {
                            Id = new Guid("850c6b56-183b-4cff-bb56-aaf267cada9d"),
                            Staat = "uitgeschreven"
                        },
                        new
                        {
                            Id = new Guid("8a421e86-0b31-43ec-99e1-346432f3053a"),
                            Staat = "onderhoud nodig"
                        });
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Tankkaart", b =>
                {
                    b.Property<string>("Kaartnummer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("GeldigheidsDatum")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("date");

                    b.Property<bool>("IsGearchiveerd")
                        .HasColumnType("bit");

                    b.Property<bool>("IsGeblokkeerd")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LaatstGeupdate")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<int>("Pincode")
                        .HasMaxLength(8)
                        .HasColumnType("int");

                    b.HasKey("Kaartnummer");

                    b.ToTable("Tankkaarten");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.ToewijzingBrandstofTankkaart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BrandstofId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Tankkaartnummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BrandstofId");

                    b.HasIndex("Tankkaartnummer");

                    b.ToTable("Toewijzingen_Brandstof_Tankkaart");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.ToewijzingRijbewijsBestuurder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RijbewijsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Rijksregisternummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("RijbewijsId");

                    b.HasIndex("Rijksregisternummer");

                    b.ToTable("Toewijzingen_Rijbewijs_Bestuurder");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Tankkaart", b =>
                {
                    b.Property<string>("Kaartnummer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("GeldigheidsDatum")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("date");

                    b.Property<bool>("IsGearchiveerd")
                        .HasColumnType("bit");

                    b.Property<bool>("IsGeblokkeerd")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LaatstGeupdate")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<int>("Pincode")
                        .HasMaxLength(8)
                        .HasColumnType("int");

                    b.HasKey("Kaartnummer");

                    b.ToTable("Tankkaarten");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.ToewijzingBrandstofTankkaart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BrandstofId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Tankkaartnummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BrandstofId");

                    b.HasIndex("Tankkaartnummer");

                    b.ToTable("Toewijzingen_Brandstof_Tankkaart");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.ToewijzingRijbewijsBestuurder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RijbewijsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Rijksregisternummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("RijbewijsId");

                    b.HasIndex("Rijksregisternummer");

                    b.ToTable("Toewijzingen_Rijbewijs_Bestuurder");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Voertuig", b =>
                {
                    b.Property<string>("Chassisnummer")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AantalDeuren")
                        .HasMaxLength(2)
                        .HasColumnType("int");

                    b.Property<int>("Bouwjaar")
                        .HasColumnType("int");

                    b.Property<Guid>("BrandstofId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategorieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsGearchiveerd")
                        .HasColumnType("bit");

                    b.Property<string>("Kleur")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("LaatstGeupdate")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("Merk")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nummerplaat")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Chassisnummer");

                    b.HasIndex("BrandstofId");

                    b.HasIndex("CategorieId");

                    b.HasIndex("StatusId");

                    b.ToTable("Voertuigen");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Bestuurder", b =>
                {
                    b.OwnsOne("Fleetmanagement_app_Groep1.Entities.Adres", "Adres", b1 =>
                        {
                            b1.Property<string>("BestuurderRijksregisternummer")
                                .HasColumnType("nvarchar(11)");

                            b1.Property<int>("Huisnummer")
                                .HasColumnType("int");

                            b1.Property<int>("Postcode")
                                .HasMaxLength(50)
                                .HasColumnType("int");

                            b1.Property<string>("Stad")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Straat")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("BestuurderRijksregisternummer");

                            b1.ToTable("Bestuurder");

                            b1.WithOwner("Bestuurder")
                                .HasForeignKey("BestuurderRijksregisternummer");

                            b1.Navigation("Bestuurder");
                        });

                    b.Navigation("Adres");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Koppeling", b =>
                {
                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Voertuig", "Voertuig")
                        .WithOne("Koppeling")
                        .HasForeignKey("Fleetmanagement_app_Groep1.Entities.Koppeling", "Chassisnummer");

                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Tankkaart", "Tankkaart")
                        .WithOne("Koppeling")
                        .HasForeignKey("Fleetmanagement_app_Groep1.Entities.Koppeling", "Kaartnummer");

                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Bestuurder", "Bestuurder")
                        .WithOne("Koppeling")
                        .HasForeignKey("Fleetmanagement_app_Groep1.Entities.Koppeling", "Rijksregisternummer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bestuurder");

                    b.Navigation("Tankkaart");

                    b.Navigation("Voertuig");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.ToewijzingBrandstofTankkaart", b =>
                {
                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Brandstof", "Brandstof")
                        .WithMany("Toewijzingen")
                        .HasForeignKey("BrandstofId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Tankkaart", "Tankkaart")
                        .WithMany("MogelijkeBrandstoffen")
                        .HasForeignKey("Tankkaartnummer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brandstof");

                    b.Navigation("Tankkaart");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.ToewijzingRijbewijsBestuurder", b =>
                {
                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Rijbewijs", "Rijbewijs")
                        .WithMany("ToewijzingenBestuurder")
                        .HasForeignKey("RijbewijsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Bestuurder", "Bestuurder")
                        .WithMany("ToewijzingenRijbewijs")
                        .HasForeignKey("Rijksregisternummer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bestuurder");

                    b.Navigation("Rijbewijs");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Voertuig", b =>
                {
                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Brandstof", "Brandstof")
                        .WithMany("Voertuigen")
                        .HasForeignKey("BrandstofId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Categorie", "Categorie")
                        .WithMany("Voertuigen")
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fleetmanagement_app_Groep1.Entities.Status", "Status")
                        .WithMany("Voertuigen")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brandstof");

                    b.Navigation("Categorie");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Bestuurder", b =>
                {
                    b.Navigation("Koppeling");

                    b.Navigation("ToewijzingenRijbewijs");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Brandstof", b =>
                {
                    b.Navigation("Toewijzingen");

                    b.Navigation("Voertuigen");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Categorie", b =>
                {
                    b.Navigation("Voertuigen");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Rijbewijs", b =>
                {
                    b.Navigation("ToewijzingenBestuurder");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Status", b =>
                {
                    b.Navigation("Voertuigen");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Tankkaart", b =>
                {
                    b.Navigation("Koppeling");

                    b.Navigation("MogelijkeBrandstoffen");
                });

            modelBuilder.Entity("Fleetmanagement_app_Groep1.Entities.Voertuig", b =>
                {
                    b.Navigation("Koppeling");
                });
#pragma warning restore 612, 618
        }
    }
}
