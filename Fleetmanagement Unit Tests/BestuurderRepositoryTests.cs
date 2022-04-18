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
    public class BestuurderRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
        private BestuurderRepository _repo;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public BestuurderRepositoryTests()
        {
            _repo = new BestuurderRepository(_context, _loggerFactory.CreateLogger("VoertuigRepositoryTests"));

            if (!_context.Database.CanConnect())
            {
                _context.Database.EnsureCreated();
                _context.Database.Migrate();
            }
        }

        [Fact]
        public async Task GetBestuurderAsync_Success_Test()
        {
            await Cleanup();
            await PopulateDataAsync();

            // Act
            var bestuurders = await _repo.GetAll();

            // Assert
            Assert.Equal(3, (bestuurders as IList<Bestuurder>).Count);
        }

        //[Fact]
        //public async Task GetBestuurderByIdAsync_Success_Test()
        //{
        //    await Cleanup();
        //    await PopulateDataAsync();

        //    // Act
        //    var bestuurder = await _repo.GetById(1.ToString());

        //    // Assert
        //    Assert.NotNull(bestuurder);
        //    Assert.Equal("Bestuurder1", bestuurder.Naam);
        //    Assert.Equal("AchternaamBestuurder1", bestuurder.Achternaam);
        //}

        [Fact]
        public async Task GetDriverLicensesForDriver_Success_Test()
        {
            await Cleanup();
            await PopulateDataAsync();

            // Act
            var rijbewijzen = await _repo.GetDriverLicensesForDriver("84060103993");

            // Assert
            Assert.NotNull(rijbewijzen);
            Assert.Single(rijbewijzen);
        }

        [Fact]
        public async Task CreateAsync_Success_Test()
        {
            await Cleanup();
            await PopulateDataAsync();

            // Act
            Bestuurder bestuurder = new Bestuurder($"bestuurder4", $"AchternaamBestuurder4", new DateTime(4 / 06 / 1984), $"84060403993");
            Adres adres = new Adres()
            {
                Huisnummer = 4,
                Straat = "Kouter",
                Postcode = 9000,
                Stad = "Gent"
            };

            bestuurder.Adres = adres;
            Rijbewijs r = new Rijbewijs() { TypeRijbewijs = "X++" };
            bestuurder.Rijbewijzen.Add(r);
            await _repo.Add(bestuurder);
            await _context.SaveChangesAsync();

            // Assert
            var bestuurders = await _repo.GetAll();
            Assert.Equal(4, (bestuurders as IList<Bestuurder>).Count);
        }

        [Fact]
        public async Task CreateAsync_Failure_Test()
        {
            await Cleanup();
            await PopulateDataAsync();

            // Act
            Bestuurder bestuurder = new Bestuurder($"", $"", DateTime.MinValue, $"");
            Adres adres = new Adres();
            bestuurder.Adres = adres;
            Rijbewijs r = new Rijbewijs();
            bestuurder.Rijbewijzen.Add(r);



            // Assert
            Assert.False(await _repo.Add(bestuurder));
            var bestuurders = await _repo.GetAll();
            Assert.Equal(3, (bestuurders as IList<Bestuurder>).Count);
        }


        private async Task PopulateDataAsync()
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
                Rijbewijs r = new Rijbewijs() { TypeRijbewijs = "X++" };
                bestuurder.Rijbewijzen.Add(r);

                index++;
                await _repo.Add(bestuurder);
                await _context.SaveChangesAsync();
            };
        }

        private async Task Cleanup()
        {
            var bestuurders = _context.Bestuurders.ToList();

            _context.Bestuurders.RemoveRange(bestuurders);

            _context.SaveChanges();
        }
    }
}