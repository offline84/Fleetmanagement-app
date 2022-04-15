using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using Fleetmanagement_app_BLL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Fleetmanagement_Unit_Tests
{
    public class TankkaartRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions());
        private ILoggerFactory _loggerfactory = new LoggerFactory();
        private TankkaartRepository _repo;

        public TankkaartRepositoryTests()
        {
            _repo = new TankkaartRepository(_context, _loggerfactory.CreateLogger("TankkaartTestlogs"));
        }

        internal Tankkaart TestCreateTankkaart(string kaartnummer, DateTime geldigheidsDatum, int pincode)
        {
            var tankkaart = new Tankkaart(kaartnummer, geldigheidsDatum);
            if (pincode > 0) { tankkaart.Pincode = pincode; }
            return tankkaart;
        }

        internal Tankkaart GetTankkaart1()
        {
            var tankkaart = new Tankkaart("123456789", DateTime.Now);
            tankkaart.Pincode = 1234;
            var toewijzingen = new List<ToewijzingBrandstofTankkaart>();

            ToewijzingBrandstofTankkaart toewijzing1 = new ToewijzingBrandstofTankkaart()
            {
                Tankkaart = tankkaart,
                Brandstof = _context.Brandstof.Where(b => b.TypeBrandstof == "euro 95").Single()
            };
            ToewijzingBrandstofTankkaart toewijzing2 = new ToewijzingBrandstofTankkaart()
            {
                Tankkaart = tankkaart,
                Brandstof = _context.Brandstof.Where(b => b.TypeBrandstof == "euro 98").Single()
            };
            toewijzingen.Add(toewijzing1);
            toewijzingen.Add(toewijzing2);
            tankkaart.MogelijkeBrandstoffen = toewijzingen;

            return tankkaart;
        }

        internal void Cleanup(Tankkaart tankkaart)
        {
            var tankkaartToDelete = _context.Tankkaarten.Where(t => t.Kaartnummer == tankkaart.Kaartnummer).AsNoTracking().FirstOrDefault();

            if (tankkaartToDelete != null)
            {
                _context.Tankkaarten.Remove(tankkaartToDelete);
            }
        }

        [Theory]
        [InlineData("123456789", "11/04/2022", 12345)]
        [InlineData("15645", "21/12/2021", 123456)]
        [InlineData("98jhjg84", "01/01/2020", 954865)]
        public async Task AddTankkaart_TankkaartIsCreated_Aangemaakt(string kaartnummer, string geldigheidsDatumString, int pincode)
        {
            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode);

            await _repo.Add(tankkaart);
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(addedTankkaart);
            Cleanup(addedTankkaart);
        }

        [Theory]
        [InlineData("123456789", 12345)]
        [InlineData("15645", 123456)]
        [InlineData("98jhjg84", 954865)]
        public async Task AddTankkaart_TankkaartZonderGeldigheidsDatum_NietAangemaakt(string kaartnummer, int pincode)
        {
            DateTime geldigheidsDatum = default;
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode);

            await _repo.Add(tankkaart);
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.Null(addedTankkaart);
            Cleanup(addedTankkaart);
        }

        [Theory]
        [InlineData("11/04/2022", 12345)]
        [InlineData("21/12/2021", 123456)]
        [InlineData("01/01/2020", 954865)]
        public async Task AddTankkaart_TankkaartZonderKaartnummer_NietAangemaakt(string geldigheidsDatumString, int pincode)
        {
            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            var kaartnummer = "";
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode);

            await _repo.Add(tankkaart);
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.Null(addedTankkaart);
            Cleanup(addedTankkaart);
        }

        [Fact]
        public async Task AddTankkaart_TankkaartBrandstofToekkeningen_Aangemaakt()
        {
            var tankkaart = GetTankkaart1();

            await _repo.Add(tankkaart);
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(addedTankkaart);
            Assert.True(addedTankkaart.MogelijkeBrandstoffen.Count() == 2);
            Cleanup(addedTankkaart);
        }


        [Fact]
        public async Task DeleteTankkaart_TankkaartWordtGearchiveerd()
        {
            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);
            await _repo.Delete(tankkaart.Kaartnummer);
            var deletedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(addedTankkaart);
            Assert.NotNull(deletedTankkaart);
            Assert.True(deletedTankkaart.IsGearchiveerd = true);
            Cleanup(addedTankkaart);
        }
        [Fact]
        public async Task GetAllActief_NaDeleteEenMinderDanGetAll()
        {
            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);
            var tankkaarten = await _repo.GetAll();
            var tankkaartenActief = await _repo.GetAllActief();

            //Probleem met GetAll?
            Assert.True(tankkaarten.Count() == tankkaartenActief.Count() + 1 );
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);
            Cleanup(addedTankkaart);
        }

        [Fact]
        public async Task Update_()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Find_()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Blokeren_()
        {
            throw new NotImplementedException();
        }


    }
}
