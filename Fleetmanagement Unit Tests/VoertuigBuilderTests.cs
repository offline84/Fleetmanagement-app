using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_Groep1.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class VoertuigBuilderTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
        private static readonly ILogger _logger;
        private VoertuigRepository _repo = new VoertuigRepository(_context, _logger);

        public VoertuigBuilderTests()
        {
            if (!_context.Database.CanConnect())
            {
                _context.Database.EnsureCreated();
                _context.Database.Migrate();
            }
        }

        internal Voertuigbuilder GetVoertuig1()
        {
            var brandstof = _context.Brandstof.FirstOrDefault();
            var status = _context.Status.Where(s => s.Staat == "in bedrijf").FirstOrDefault();
            var categorie = _context.Categorie.FirstOrDefault();

            var voertuig = new Voertuigbuilder()
            {
                Chassisnummer = "VF3 7BRFVE12345678",
                Merk = "Ford",
                Nummerplaat = "VNG 746",
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("      ")]
        public void StringVariabelenInConstructorMogenNietEmptyZijn(string value)
        {
            var voertuig = GetVoertuig1();
            voertuig.Chassisnummer = value;

            Assert.False(voertuig.IsValid());
            Assert.Throws<InvalidOperationException>(() => voertuig.Build());

            voertuig = GetVoertuig1();
            voertuig.Merk = value;

            Assert.False(voertuig.IsValid());
            Assert.Throws<InvalidOperationException>(() => voertuig.Build());

            voertuig = GetVoertuig1();
            voertuig.Model = value;

            Assert.False(voertuig.IsValid());
            Assert.Throws<InvalidOperationException>(() => voertuig.Build());
        }

        [Fact]
        public void VoertuigMoetCategorieBevatten()
        {
            var voertuig = GetVoertuig1();
            voertuig.Categorie = null;

            Assert.False(voertuig.IsValid());
            Assert.Throws<InvalidOperationException>(() => voertuig.Build());
        }

        [Fact]
        public void VoertuigMoetBrandstofBevatten()
        {
            var voertuig = GetVoertuig1();
            voertuig.Brandstof = null;

            Assert.False(voertuig.IsValid());
            Assert.Throws<InvalidOperationException>(() => voertuig.Build());
        }

        [Fact]
        public void GeenNummerplaatKanEnkelAlsStatusIsInAankoop()
        {
            var bouwvoertuig = GetVoertuig1();
            bouwvoertuig.Nummerplaat = "";

            Assert.Throws<InvalidOperationException>(() => bouwvoertuig.Build());

            bouwvoertuig.Status = _context.Status.Where(s => s.Staat == "aankoop").FirstOrDefault();
            var voertuig = bouwvoertuig.Build();

            Assert.NotNull(voertuig);

            bouwvoertuig.Nummerplaat = "ABC-123";
            voertuig = bouwvoertuig.Build();

            Assert.NotNull(voertuig);
        }

        [Fact]
        public void GeenStatus_InAankoop_ZonderNummerplaatGeeftFoutmelding()
        {
            var bouwvoertuig = GetVoertuig1();
            bouwvoertuig.Nummerplaat = "";
            bouwvoertuig.Status = null;

            Assert.Throws<InvalidOperationException>(() => bouwvoertuig.Build());
        }
    }
}