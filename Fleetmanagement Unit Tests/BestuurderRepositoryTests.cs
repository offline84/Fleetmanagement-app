using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using FleetmanagementApp.BUL.Repository;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class BestuurderRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions());
        private static readonly ILogger _logger;
        private BestuurderRepository _bestuurdersRepository = new BestuurderRepository(_context, _logger);

        internal Bestuurder GetBestuurder1()
        {
            var bestuurder = new Bestuurder("Maiko", "Samyn", new DateTime(17/06/1984), "84061703993");

            Adres adres = new Adres()
            {
                Huisnummer = 61,
                Straat = "Kouter",
                Postcode = 9000,
                Stad = "Gent"
            };

            bestuurder.Adres = adres;
            Rijbewijs r = new Rijbewijs(){TypeRijbewijs = "X++"};

            bestuurder.Rijbewijzen.Add(r);

            return bestuurder;
        }
       
        [Fact]
        public async void AddingBestuurderToDBTest()
        {
            //Arrange
            
            var bestuurder = this.GetBestuurder1();
            
            // Assert

            Assert.NotNull(bestuurder);
            Assert.Equal(61, bestuurder.Adres.Huisnummer);

            Assert.True(await _bestuurdersRepository.Add(bestuurder)); 
        }


        [Fact]
        public async void BestuurderMoetNaamHebbenTest()
        {
            var bestuurder = GetBestuurder1();
            bestuurder.Naam = "";

            Assert.False(await _bestuurdersRepository.Add(bestuurder));

            bestuurder.Achternaam = " ";

            Assert.False(await _bestuurdersRepository.Add(bestuurder));

            bestuurder = GetBestuurder1();
            bestuurder.Achternaam = "";

            Assert.False(await _bestuurdersRepository.Add(bestuurder));

            bestuurder.Achternaam = " ";

            Assert.False(await _bestuurdersRepository.Add(bestuurder));
        }
    }
}
