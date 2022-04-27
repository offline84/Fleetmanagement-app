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
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
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

        internal void Cleanup()
        {
            var tankkaarten = _context.Tankkaarten.ToList();
            var toewijzingen = _context.ToewijzingBrandstofTankkaarten.ToList();

            _context.Tankkaarten.RemoveRange(tankkaarten);
            _context.ToewijzingBrandstofTankkaarten.RemoveRange(toewijzingen);
            _context.SaveChanges();
        }

        [Theory]
        [InlineData("123456789", "11/04/2022", 12345)]
        [InlineData("15645", "21/12/2021", 123456)]
        [InlineData("98jhjg84", "01/01/2020", 954865)]
        public async Task AddTankkaart_TankkaartIsCreated_Aangemaakt(string kaartnummer, string geldigheidsDatumString, int pincode)
        {
            Cleanup();

            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode);

            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(addedTankkaart);
        }

        [Theory]
        [InlineData("123456789", 12345)]
        [InlineData("15645", 123456)]
        [InlineData("98jhjg84", 954865)]
        public async Task AddTankkaart_TankkaartZonderGeldigheidsDatum_NietAangemaakt(string kaartnummer, int pincode)
        {
            Cleanup();

            DateTime geldigheidsDatum = default;
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode);

            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.Null(addedTankkaart);
        }

        [Theory]
        [InlineData("11/04/2022", 12345)]
        [InlineData("21/12/2021", 123456)]
        [InlineData("01/01/2020", 954865)]
        public async Task AddTankkaart_TankkaartZonderKaartnummer_NietAangemaakt(string geldigheidsDatumString, int pincode)
        {
            Cleanup();

            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            var kaartnummer = "";
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode);

            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.Null(addedTankkaart);
        }

        [Fact]
        public async Task AddTankkaart_TankkaartBrandstofToekkeningen_Aangemaakt()
        {
            Cleanup();

            var tankkaart = GetTankkaart1();

            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(addedTankkaart);
            Assert.True(addedTankkaart.MogelijkeBrandstoffen.Count() == 2);
        }


        [Fact]
        public async Task DeleteTankkaart_TankkaartWordtGearchiveerd()
        {
            Cleanup();

            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);
            await _repo.Delete(tankkaart.Kaartnummer);
            await _context.SaveChangesAsync();
            var deletedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(addedTankkaart);
            Assert.NotNull(deletedTankkaart);
            Assert.True(deletedTankkaart.IsGearchiveerd = true);
        }

        [Fact]
        public async Task Delete_LinkNaarKoppelingVerwijderd()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetAllActief_NaDeleteEenMinderDanGetAll()
        {
            Cleanup();

            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            await _repo.Delete(tankkaart.Kaartnummer);
            await _context.SaveChangesAsync();
            var tankkaarten = await _repo.GetAll();
            var aantalTankkaarten = tankkaarten.Count();
            var tankkaartenActief = await _repo.GetAllActief();
            var aantalTankkaartenActief = tankkaartenActief.Count();

            Assert.True(aantalTankkaarten == aantalTankkaartenActief + 1);
        }

        [Fact]
        public async Task Update_UpdateWerkt()
        {
            Cleanup();
            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);

            tankkaart.Pincode = 9876;
            await _repo.Update(tankkaart);
            await _context.SaveChangesAsync();

            var geupdateTankaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.Equal(geupdateTankaart.Kaartnummer, tankkaart.Kaartnummer);
        }

        [Fact]
        public async Task Update_()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public async Task Find_ZoekenNaarVoertuig()
        {
            Cleanup();

            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();

            var zoekResultaat = await _repo.Find(t => t.Pincode == 1234);

            Assert.Equal(tankkaart, zoekResultaat.First());
        }

        [Fact]
        public async Task Blokeren_IsGeblokeerd()
        {
            Cleanup();

            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            await _repo.Blokkeren(tankkaart.Kaartnummer);
            await _context.SaveChangesAsync();
            var geblokkerdeTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.True(geblokkerdeTankkaart.IsGeblokkeerd);
        }

        [Fact]
        public async Task Blokeren_AlGeblokeerd()
        {
            Cleanup();

            var tankkaart = GetTankkaart1();
            tankkaart.IsGeblokkeerd = true;
            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            await _repo.Blokkeren(tankkaart.Kaartnummer);
            await _context.SaveChangesAsync();
            var geblokkerdeTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.True(geblokkerdeTankkaart.IsGeblokkeerd);
        }

    }
}
