using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class VoertuigRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
        private VoertuigRepository _repo;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public VoertuigRepositoryTests()
        {
            _repo = new VoertuigRepository(_context, _loggerFactory.CreateLogger("VoertuigRepositoryTests"));
            
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

            var voertuig = new Voertuigbuilder(_repo)
            {
                Chassisnummer = "VF37BRFVE12345678",
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
            var status = _context.Status.Where(s => s.Staat == "aankoop").FirstOrDefault();
            var categorie = _context.Categorie.FirstOrDefault();

            var voertuig = new Voertuigbuilder(_repo)
            {
                Chassisnummer = "WB66RFDE87654321",
                Merk = "Opel",
                Nummerplaat = "ABC123",
                Model = "Vectra",
                Bouwjaar = 1992,
                AantalDeuren = 5,
                Kleur = "blauw",
                Categorie = categorie,
                Status = status,
                Brandstof = brandstof
            };

            return voertuig;
        }

        internal void Cleanup()
        {
            var voertuigen = _context.Voertuigen.ToList();

            _context.Voertuigen.RemoveRange(voertuigen);
            
            _context.SaveChanges();
        }

        [Fact]
        public async void EenLegeTabelVoertuigGeeftGeenVoertuig()
        {
            Cleanup();

            var voertuigen = await _repo.GetAll();

            Assert.False(voertuigen.Any());
        }

        [Fact]
        public async void VoertuigWordtCorrectToegevoegdAanDeDatabase()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var requestedvehicle = await _repo.GetById(voertuig.Chassisnummer);

            Assert.NotNull(requestedvehicle);
        }

        [Fact]
        public async void ToevoegenVanVoertuigVultLaatstGeupdateAutomatischIn()
        {
            Cleanup();

            var first = DateTime.Now;
            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var requestedvehicle = await _repo.GetById(voertuig.Chassisnummer);

            Assert.InRange(requestedvehicle.LaatstGeupdate, first, DateTime.Now);

        }

        [Fact]
        public async void GetAllArchivedGeeftEnkelGearchiveerdeVoertuigenWeer()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig1 = builder.Build(); 
            await _repo.Add(voertuig1);
            await _context.SaveChangesAsync();

            var b2 = GetVoertuig2();
            var voertuig2 = b2.Build();
            await _repo.Add(voertuig2);
            await _context.SaveChangesAsync();

            await _repo.Delete(voertuig1.Chassisnummer);
            await _context.SaveChangesAsync();

            var archived = await _repo.GetAllArchived();
            Assert.Single(archived);
            Assert.Equal(archived.First().Nummerplaat, voertuig1.Nummerplaat);
        }

        [Fact]
        public async void GetAllActiveGeeftEnkelActieveVoertuigenWeer()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig1 = builder.Build(); 
            await _repo.Add(voertuig1);
            await _context.SaveChangesAsync();

            var b2 = GetVoertuig2();
            var voertuig2 = b2.Build();
            await _repo.Add(voertuig2);
            await _context.SaveChangesAsync();

            await _repo.Delete(voertuig1.Chassisnummer);
            await _context.SaveChangesAsync();

            var active = await _repo.GetAllActive();
            Assert.Single(active);
            Assert.Equal(active.First().Nummerplaat, voertuig2.Nummerplaat);
        }

        [Fact]
        public async void EenChassisnummerMagMaarAan1WagenToegekendZijnEnMaaktToevoegenVanEenTweedeZelfdeOnmogelijk()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var builder2 = GetVoertuig2();
            builder2.Chassisnummer = builder.Chassisnummer;
            var voertuig2 = builder2.Build();

            var isAdded = await _repo.Add(voertuig2);

            Assert.False(isAdded);
        }

        [Fact]
        public async void EenNummerplaatMagMaarAan1WagenToegekendZijnEnMaaktToevoegenVanEenTweedeZelfdeOnmogelijk()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var builder2 = GetVoertuig2();
            builder2.Nummerplaat = builder.Nummerplaat;
            var voertuig2 = builder2.Build();

            var isAdded = await _repo.Add(voertuig2);

            Assert.False(isAdded);
        }

        [Fact]
        public async void UpdatenVanEenVoertuigIsMogelijk()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            

            builder.AantalDeuren = 5;
            builder.Kleur = "groen";
            voertuig = builder.Build();
            await _context.SaveChangesAsync();

            var update = await _repo.Update(voertuig);
            await _context.SaveChangesAsync();

            var voertuigen = await _repo.GetAll();
            var updatedVoertuig = voertuigen.Where(v => v.Chassisnummer == voertuig.Chassisnummer).FirstOrDefault();
           
            Assert.True(update);
            Assert.Equal("groen", updatedVoertuig.Kleur);
            Assert.Equal(5, updatedVoertuig.AantalDeuren);
        }

        [Fact]
        public async void UpdatenVanEenVoertuigIsEnkelMogelijkAlsEenVoertuigMetZelfdeChassisNummerAanwezigIs()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            var isAdded = await _repo.Update(voertuig);
            var voertuigen = await _repo.GetAll();
            await _context.SaveChangesAsync();

            Assert.False(isAdded);
            Assert.Empty(voertuigen);

        }

        [Fact]
        public async void UpdateBehandeltCategorieInVoertuigCorrect()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            var EersteVoertuig = builder.Build();

            await _repo.Add(voertuig);
            _context.SaveChanges();

            var categorien = await _context.Set<Categorie>().ToListAsync() ;
            voertuig.Categorie = categorien[2];

            var isAdded = await _repo.Update(voertuig);
            _context.SaveChanges();

            var getVoertuigen = await _repo.GetAll();
            var updatedVoertuig = getVoertuigen.First();

            Assert.True(isAdded);

            Assert.Equal(voertuig.CategorieId, updatedVoertuig.CategorieId);
            Assert.NotEqual(voertuig.CategorieId, EersteVoertuig.CategorieId);
            
        }

        [Fact]
        public async void UpdateBehandeltStatusInVoertuigCorrect()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            var EersteVoertuig = builder.Build();

            await _repo.Add(voertuig);
            _context.SaveChanges();

            var statussen = await _context.Set<Status>().ToListAsync() ;
            voertuig.Status= statussen[2];

            var isAdded = await _repo.Update(voertuig);
            _context.SaveChanges();

            var getVoertuigen = await _repo.GetAll();
            var updatedVoertuig = getVoertuigen.First();

            Assert.True(isAdded);

            Assert.Equal(voertuig.StatusId, updatedVoertuig.StatusId);
            Assert.NotEqual(voertuig.StatusId, EersteVoertuig.StatusId);
            
        }

         [Fact]
        public async void UpdateBehandeltBrandstofInVoertuigCorrect()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            var EersteVoertuig = builder.Build();

            await _repo.Add(voertuig);
            _context.SaveChanges();

            var brandstoffen = await _context.Set<Brandstof>().ToListAsync() ;
            voertuig.Brandstof= brandstoffen[2];

            var isAdded = await _repo.Update(voertuig);
            _context.SaveChanges();

            var getVoertuigen = await _repo.GetAll();
            var updatedVoertuig = getVoertuigen.First();

            Assert.True(isAdded);

            Assert.Equal(voertuig.BrandstofId, updatedVoertuig.BrandstofId);
            Assert.NotEqual(voertuig.BrandstofId, EersteVoertuig.BrandstofId);          
        }

        [Fact]
        public async void UpdatePastVeldLaatstGeupdateAan()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            var added = _repo.GetById(voertuig.Chassisnummer).Result.LaatstGeupdate;
            await _context.SaveChangesAsync();

            voertuig.Kleur = "grijs";
            voertuig.Bouwjaar = 2022;

            Thread.Sleep(5000);
            await _repo.Update(voertuig);
            await _context.SaveChangesAsync();
            var update = _repo.GetById(voertuig.Chassisnummer).Result.LaatstGeupdate;

            Assert.NotEqual(added, update);
            Assert.True(added < update);
        }

        [InlineData("")]
        [InlineData(" ")]
        [InlineData("       ")]
        [Theory]
        public async void UpdateGeeftFalseBijLegeVerplichteVelden(string veld)
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            voertuig.Merk = veld;

            Assert.False( await _repo.Update(voertuig));

            voertuig = builder.Build();
            voertuig.Model = veld;

            Assert.False( await _repo.Update(voertuig));

            voertuig = builder.Build();
            voertuig.Nummerplaat = veld;

            Assert.False( await _repo.Update(voertuig));

        }

        [Fact]
        public async void DeletenArchiveertVoertuig()
        {
             Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            await _repo.Delete(voertuig.Chassisnummer);

            var inArchief = _repo.GetById(voertuig.Chassisnummer).Result;

            Assert.True(inArchief.IsGearchiveerd);
        
        }

        [Fact]
        public async void ZoekenNaarBepaaldeVoertuigenWerkt()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);

            builder = GetVoertuig2();
            voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var search = await _repo.Find(v => v.Kleur == "blauw");

            Assert.True(search.Any());
            Assert.Equal(voertuig, search.First());

        }

        [Fact]
        public async void ZoekenNaarOnbestaandVeldGeeftNull()
        {
            Cleanup();

            var builder = GetVoertuig1();
            var voertuig = builder.Build();

            await _repo.Add(voertuig);

            builder = GetVoertuig2();
            voertuig = builder.Build();

            await _repo.Add(voertuig);
            await _context.SaveChangesAsync();

            var search = await _repo.Find(v => v.Kleur == "oranje");

            Assert.Empty(search);

        }
    }
}