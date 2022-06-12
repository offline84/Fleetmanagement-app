using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using Fleetmanagement_app_BLL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
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

        internal Tankkaart TestCreateTankkaart(string kaartnummer, DateTime geldigheidsDatum, int pincode, string brandstoffen)
        {
            var tankkaart = new Tankkaart(kaartnummer, geldigheidsDatum);
            if (pincode > 0) { tankkaart.Pincode = pincode; }
            var toewijzingen = CreateToewijzingen(brandstoffen);
            tankkaart.MogelijkeBrandstoffen = toewijzingen;
            return tankkaart;
        }

        internal Tankkaart GetTankkaart1()
        {
            var tankkaart = new Tankkaart("123456789", DateTime.Now);
            tankkaart.Pincode = 1234;

            var toewijzingen = CreateToewijzingen("euro 95,euro 95");
            tankkaart.MogelijkeBrandstoffen = toewijzingen;

            return tankkaart;
        }

        internal List<ToewijzingBrandstofTankkaart> CreateToewijzingen(string brandstoffen)
        {
            var toewijzingen = new List<ToewijzingBrandstofTankkaart>();
 
            if (brandstoffen != "")
            {
                var arrayBrandstoffen = brandstoffen.Split(',');
                foreach (var brandstof in arrayBrandstoffen)
                {
                    ToewijzingBrandstofTankkaart toewijzing = new ToewijzingBrandstofTankkaart()
                    {
                        Brandstof = _context.Brandstof.Where(b => b.TypeBrandstof == brandstof).Single()
                    };
                    toewijzingen.Add(toewijzing);
                }
            }

            return toewijzingen;
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
        [InlineData("123456789", "11/04/2022", 9999,"")]
        [InlineData("15645", "21/12/2021", 123456, "euro 95,euro 95")]
        [InlineData("98jhjg84", "01/01/2029", 213322, "AdBlue")]
        [InlineData("7894456", "06/04/2024", 12345, "")]
        [InlineData("AF44784", "15/12/2021", 1234, "euro 95,euro 95")]
        [InlineData("HRY87654", "02/05/2020", 954865, "AdBlue")]
        public async Task AddTankkaart_TankkaartIsCreated_Aangemaakt(string kaartnummer, string geldigheidsDatumString, int pincode, string brandstoffen)
        {
            Cleanup();

            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode, brandstoffen);

            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(addedTankkaart);
        }

        [Theory]
        [InlineData("123456789", 9999, "")]
        [InlineData("15645", 123456, "euro 95,euro 95")]
        [InlineData("98jhjg84", 213322, "AdBlue")]
        [InlineData("7894456", 12345, "")]
        [InlineData("AF44784", 1234, "euro 95,euro 95")]
        [InlineData("HRY87654", 954865, "AdBlue")]
        public async Task AddTankkaart_TankkaartZonderGeldigheidsDatum_NietAangemaakt(string kaartnummer, int pincode, string brandstoffen)
        {
            Cleanup();

            DateTime geldigheidsDatum = default;
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode, brandstoffen);

            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();
            var addedTankkaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.Null(addedTankkaart);
        }

        [Theory]
        [InlineData("11/04/2022", 9999, "")]
        [InlineData("21/12/2021", 123456, "euro 95,euro 95")]
        [InlineData("01/01/2029", 213322, "AdBlue")]
        [InlineData("06/04/2024", 12345, "")]
        [InlineData("15/12/2021", 1234, "euro 95,euro 95")]
        [InlineData( "02/05/2020", 954865, "AdBlue")]
        public async Task AddTankkaart_TankkaartZonderKaartnummer_NietAangemaakt(string geldigheidsDatumString, int pincode, string brandstoffen)
        {
            Cleanup();

            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            var kaartnummer = "";
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode, brandstoffen);

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

        [Theory]
        [InlineData("123456789", "11/04/2022", 9999, "")]
        [InlineData("123456789", "21/12/2021", 123456, "euro 95,euro 95")]
        [InlineData("123456789", "01/01/2029", 213322, "AdBlue")]
        [InlineData("123456789", "06/04/2024", 12345, "")]
        [InlineData("123456789", "15/12/2021", 1234, "euro 95,euro 95")]
        [InlineData("123456789", "02/05/2020", 954865, "AdBlue")]
        public async Task Update_Werkt(string kaartnummer, string geldigheidsDatumString, int pincode, string brandstoffen)
        {
            Cleanup();
            var tankkaart = GetTankkaart1();
            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            await _repo.Add(tankkaart);
            await _context.SaveChangesAsync();

            var tankkaartToUpdate = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode, brandstoffen);
            await _repo.Update(tankkaartToUpdate);
            await _context.SaveChangesAsync();

            var geupdateTankaart = await _repo.GetById(tankkaart.Kaartnummer);
            
            Assert.Equal(geupdateTankaart.Kaartnummer, tankkaartToUpdate.Kaartnummer);
            Assert.Equal(geupdateTankaart.GeldigheidsDatum, tankkaartToUpdate.GeldigheidsDatum);
            Assert.Equal(geupdateTankaart.Pincode, tankkaartToUpdate.Pincode);
            Assert.Equal(geupdateTankaart.MogelijkeBrandstoffen, tankkaartToUpdate.MogelijkeBrandstoffen);
        }

        [Theory]
        [InlineData(123456)]
        [InlineData(9876)]
        [InlineData(65135)]
        public async Task UpdatePincode_UpdateWerkt(int pincode)
        {
            Cleanup();
            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);

            tankkaart.Pincode = pincode;
            await _repo.Update(tankkaart);
            await _context.SaveChangesAsync();

            var geupdateTankaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.Equal(geupdateTankaart.Kaartnummer, tankkaart.Kaartnummer);
        }

        [Fact]
        public async Task UpdateToewijzingen_BrandstofToegevoegd()
        {
            Cleanup();
            var tankkaart = GetTankkaart1();
            await _repo.Add(tankkaart);

            ToewijzingBrandstofTankkaart extraToewijzing = new ToewijzingBrandstofTankkaart()
            {
                Tankkaart = tankkaart,
                Brandstof = _context.Brandstof.Where(b => b.TypeBrandstof == "euro 98").Single()
            };

            tankkaart.MogelijkeBrandstoffen.Add(extraToewijzing);

            await _repo.Update(tankkaart);
            await _context.SaveChangesAsync();

            var geupdateTankaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(geupdateTankaart);
            Assert.True(geupdateTankaart.MogelijkeBrandstoffen.Count() == 3);
        }

        [Fact]
        public async Task UpdateToewijzingen_OudeVerwijderd()
        {
            Cleanup();
            var tankkaart = GetTankkaart1();

            ToewijzingBrandstofTankkaart nieuweToewijzing = new ToewijzingBrandstofTankkaart()
            {
                Tankkaart = tankkaart,
                Brandstof = _context.Brandstof.Where(b => b.TypeBrandstof == "euro 98").Single()
            };
            foreach (ToewijzingBrandstofTankkaart brandstof in tankkaart.MogelijkeBrandstoffen.ToList())
            {
                tankkaart.MogelijkeBrandstoffen.Remove(brandstof);
            }
           
            tankkaart.MogelijkeBrandstoffen.Add(nieuweToewijzing);

            await _repo.Add(tankkaart);
            await _repo.Update(tankkaart);
            await _context.SaveChangesAsync();

            var geupdateTankaart = await _repo.GetById(tankkaart.Kaartnummer);

            Assert.NotNull(geupdateTankaart);
            Assert.True(geupdateTankaart.MogelijkeBrandstoffen.Count() == 1);
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
