using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    //     XUnit voert iedere testclass in parallel uit met elkaar om op die manier tijd uit te sparen.
    //     Dit zorgt voor problemen doordat er van en naar de databank geschreven wordt op hetzelfde moment dat er een andere test loopt
    //     die deze data nodig heeft om de test succesvol uit te voeren.
    //     Als je een klasse decoreert met het collectionattribuut, dan zoekt de unit test collecties op met dezelfde naam en voert deze ze unilateraal uit.

    [Collection("DatabaseTests")]
    public class BestuurderRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
        private BestuurderRepository _repo;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        /// <summary>
        /// Constructor: Repo opvullen en testen indien connectie werkt/TestDatabase aangemaakt is
        /// </summary>
        public BestuurderRepositoryTests()
        {
            _repo = new BestuurderRepository(_context, _loggerFactory.CreateLogger("VoertuigRepositoryTests"));

            if (!_context.Database.CanConnect())
            {
                _context.Database.EnsureCreated();
                _context.Database.Migrate();
            }
        }

        /// <summary>
        /// Zorg voor de opkuis van de database. 
        /// </summary>
        /// <returns></returns>
        internal void Cleanup()
        {
            var tankkaarten = _context.Tankkaarten.ToList();
            var toewijzingen = _context.ToewijzingBrandstofTankkaarten.ToList();
            var bestuurders = _context.Bestuurders.ToList();
            var koppelingen = _context.Koppelingen.ToList();
            var voertuigen = _context.Voertuigen.ToList();

            _context.Tankkaarten.RemoveRange(tankkaarten);
            _context.ToewijzingBrandstofTankkaarten.RemoveRange(toewijzingen);
            _context.Bestuurders.RemoveRange(bestuurders);
            _context.Koppelingen.RemoveRange(koppelingen);
            _context.Voertuigen.RemoveRange(voertuigen);
            _context.SaveChanges();
        }

        /// <summary>
        /// Vul de database op een Async manier. 
        /// </summary>
        /// <returns></returns>
        internal async Task ToevoegenBestuurdersAsync()
        {
            int index = 1;
            while (index <= 3)
            {
                Bestuurder bestuurder = new Bestuurder($"bestuurder{index}", $"AchternaamBestuurder{index}", DateTime.Parse($"{index}/{06}/{1984}"), $"84060{index}03993");
                Adres adres = new Adres()
                {
                    Huisnummer = index,
                    Straat = "Kouter",
                    Postcode = 9000,
                    Stad = "Gent"
                };

                bestuurder.Adres = adres;
                ToevoegenToewijzingenRijbewijs(bestuurder);

                index++;
                await _repo.Add(bestuurder);
                await _context.SaveChangesAsync();
            };
        }

        /// <summary>
        /// Voeg een geldige rijbewijs aan een toewijgenRijbewijsLijst.
        /// </summary>
        /// <param name="bestuurder"></param>
        internal void ToevoegenToewijzingenRijbewijs(Bestuurder bestuurder)
        {
            Rijbewijs r = new Rijbewijs() { TypeRijbewijs = "D", Id = Guid.Parse("1247f201-ca7f-4d5f-83dd-ef8aa351ea8d") };
            var toewijzingen = new List<ToewijzingRijbewijsBestuurder>
                {
                    new ToewijzingRijbewijsBestuurder { Rijbewijs = r }
                };
            bestuurder.ToewijzingenRijbewijs = toewijzingen;
        }

        /// <summary>
        /// Opzet voor het toevoegen van een bestuurder met toewijzingenRijbewijs.
        /// </summary>
        /// <returns></returns>
        internal Bestuurder ToevoegenBestuurder()
        {
            Bestuurder bestuurder = new Bestuurder($"bestuurder4", $"AchternaamBestuurder4", new DateTime(4 / 06 / 1984), $"84060403993");
            Adres adres = new Adres()
            {
                Huisnummer = 4,
                Straat = "Kouter",
                Postcode = 9000,
                Stad = "Gent"
            };

            bestuurder.Adres = adres;
            ToevoegenToewijzingenRijbewijs(bestuurder);
            return bestuurder;
        }

        /// <summary>
        /// Test indien het ophalen van de opgemaakte bestuurders werkt.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBestuurderAsync_Success_Test()
        {
            Cleanup();
            await ToevoegenBestuurdersAsync();

            // Act
            var bestuurders = await _repo.GetAll();

            // Assert
            Assert.Equal(3, (bestuurders as IList<Bestuurder>).Count);
        }

        /// <summary>
        /// Test indien het ophalen van een bestuurder volgens Id werkt.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBestuurderByIdAsync_Success_Test()
        {
            Cleanup();
            await ToevoegenBestuurdersAsync();

            // Act
            var bestuurder = await _repo.GetById($"84060103993");

            // Assert
            Assert.NotNull(bestuurder);
            Assert.Equal("bestuurder1", bestuurder.Naam);
            Assert.Equal("AchternaamBestuurder1", bestuurder.Achternaam);
        }

        /// <summary>
        /// Test indien het opmaken van een Bestuurder lukt.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateAsync_Success_Test()
        {
            Cleanup();
            await ToevoegenBestuurdersAsync();

            // Act
            Bestuurder bestuurder = ToevoegenBestuurder();
            await _repo.Add(bestuurder);
            await _context.SaveChangesAsync();

            // Assert
            var bestuurders = await _repo.GetAll();
            Assert.NotNull(bestuurders);
            Assert.Equal("bestuurder4", bestuurder.Naam);
            Assert.Equal("AchternaamBestuurder4", bestuurder.Achternaam);
            Assert.Equal(4, (bestuurders as IList<Bestuurder>).Count);
        }

        /// <summary>
        /// Test indien het opmaken van een bestuurder faalt.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateAsync_Failure_Test()
        {
            Cleanup();
            await ToevoegenBestuurdersAsync();

            // Act
            Bestuurder bestuurder = new Bestuurder($"", $"", DateTime.MinValue, $"");
            Adres adres = new Adres();
            bestuurder.Adres = adres;
            ToevoegenToewijzingenRijbewijs(bestuurder);

            // Assert
            Assert.False(await _repo.Add(bestuurder));
            var bestuurders = await _repo.GetAll();
            Assert.Equal(3, (bestuurders as IList<Bestuurder>).Count);
        }

        /// <summary>
        /// Test indien het updaten van een bestuurder succes.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateAsync_Success_Test()
        {
            Cleanup();
            await ToevoegenBestuurdersAsync();

            // Act
            var bestuurder = await _repo.GetById("84060103993");
            bestuurder.Naam = "test1";
            bestuurder.Achternaam = "testAchternaam1";
            bestuurder.GeboorteDatum = DateTime.Parse($"{2}/{02}/{1982}");
            await _repo.Update(bestuurder);
            await _context.SaveChangesAsync();
            var bestuurderDb = await _repo.GetById("84060103993");

            // Assert
            Assert.Equal(bestuurder.Naam, bestuurderDb.Naam);
            Assert.Equal(bestuurder.Achternaam, bestuurderDb.Achternaam);
            Assert.Equal(bestuurder.GeboorteDatum, bestuurderDb.GeboorteDatum);
        }

        /// <summary>
        /// Test indien het updaten van een bestuurder-adres succes.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateOwnedAsync_Success_Test()
        {
            Cleanup();
            await ToevoegenBestuurdersAsync();

            // Act
            var bestuurder = await _repo.GetById("84060103993");
            bestuurder.Adres.Stad = "testStad";
            bestuurder.Adres.Straat = "testStraat";
            bestuurder.Adres.Postcode = 1500;
            bestuurder.Adres.Huisnummer = 150;
            await _repo.Update(bestuurder);
            await _context.SaveChangesAsync();
            var bestuurderDb = await _repo.GetById("84060103993");

            // Assert
            Assert.Equal(bestuurder.Adres.Stad, bestuurderDb.Adres.Stad);
            Assert.Equal(bestuurder.Adres.Straat, bestuurderDb.Adres.Straat);
            Assert.Equal(bestuurder.Adres.Postcode, bestuurderDb.Adres.Postcode);
            Assert.Equal(bestuurder.Adres.Huisnummer, bestuurderDb.Adres.Huisnummer);
        }

        /// <summary>
        /// Test indien het archive van een bestuurder succes.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ArchiveAsync_Success_Test()
        {
            Cleanup();
            await ToevoegenBestuurdersAsync();

            // Act
            var bestuurder = await _repo.GetById("84060103993");
            if (bestuurder != null)
            {
                await _repo.Delete(bestuurder.Rijksregisternummer);
                await _context.SaveChangesAsync();
            }

            var bestuurderDb = await _repo.GetById("84060103993");

            // Assert
            Assert.NotNull(bestuurderDb);
            Assert.True(bestuurderDb.IsGearchiveerd);
        }



    }
}