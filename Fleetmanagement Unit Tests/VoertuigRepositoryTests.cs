using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class VoertuigRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions());
        private VoertuigRepository _repo;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public VoertuigRepositoryTests()
        {
            _repo = new VoertuigRepository(_context, _loggerFactory.CreateLogger("VoertuigRepositoryTests"));
        }

        internal Voertuigbuilder GetVoertuig1()
        {
            var brandstof = _context.Brandstof.FirstOrDefault();
            var status = _context.Status.Where(s => s.Staat == "in bedrijf").FirstOrDefault();
            var categorie = _context.Categorie.FirstOrDefault();

            var voertuig = new Voertuigbuilder(_repo)
            {
                Chassisnummer = "VF3 7BRFVE12345678",
                Merk = "Ford",
                Nummerplaat = "VNG746",
                Model = "Cobra",
                Bouwjaar = 1987,
                AantalDeuren = 3,
                Kleur = "midnight pink",
                Categorie = categorie,
                Status = status,
                Brandstof = brandstof
            };

            return voertuig;
        }

        internal Voertuigbuilder GetVoertuig2()
        {
            var brandstof = _context.Brandstof.FirstOrDefault();
            var status = _context.Status.Where(s => s.Staat == "in aankoop").FirstOrDefault();
            var categorie = _context.Categorie.FirstOrDefault();

            var voertuig = new Voertuigbuilder(_repo)
            {
                Chassisnummer = "VF3 7BRFVE12345678",
                Merk = "Ford",
                Nummerplaat = "ABC123",
                Model = "Cobra",
                Bouwjaar = 1987,
                AantalDeuren = 3,
                Kleur = "midnight pink",
                Categorie = categorie,
                Status = status,
                Brandstof = brandstof
            };

            return voertuig;
        }

        internal void Cleanup(Voertuig voertuig)
        {
            var distinctVoertuig = _context.Voertuigen.Where(v => v.Chassisnummer == voertuig.Chassisnummer).AsNoTracking().FirstOrDefault();

            if (distinctVoertuig != null)
            {
                _context.Voertuigen.Remove(voertuig);
                _context.SaveChanges();
            }
        }

        [Fact]
        public async void EenLegeTabelVoertuigGeeftGeenVoertuig()
        {
            var voertuigen = await _repo.GetAll();

            Assert.False(voertuigen.Any());
        }

        [Fact]
        public async void VoertuigWordtCorrectToegevoegdAanDeDatabase()
        {
            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var requestedvehicle = await _repo.GetById(voertuig.Chassisnummer);

            Assert.NotNull(requestedvehicle);

            Cleanup(voertuig);
        }

        [Fact]
        public async void EenChassisnummerMagMaarAan1WagenToegekendZijnEnMaaktToevoegenVanEenTweedeZelfdeOnmogelijk()
        {
            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            Cleanup(voertuig);

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var builder2 = GetVoertuig2();
            var voertuig2 = builder2.Build();

            var isAdded = await _repo.Add(voertuig2);

            Assert.False(isAdded);
        }

        [Fact]
        public void EenNummerplaatMagMaarAan1WagenToegekendZijnEnMaaktToevoegenVanEenTweedeZelfdeOnmogelijk()
        {
        }
    }
}