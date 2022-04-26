using Fleetmanagement_app_BLL;
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
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class SeededClassesRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
        private CategorieRepository _cats;
        private StatusRepository _stats;
        private BrandstofRepository _brands;
        private VoertuigRepository _repo;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public SeededClassesRepositoryTests()
        {
            _cats = new CategorieRepository(_context, _loggerFactory.CreateLogger("seededCategoryTest"));
            _stats = new StatusRepository(_context, _loggerFactory.CreateLogger("seededStatusTest"));
            _brands = new BrandstofRepository(_context, _loggerFactory.CreateLogger("seededBrandstofTest"));
            _repo = new VoertuigRepository(_context, _loggerFactory.CreateLogger("seededVoertuigTest"));

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

        [Fact]
        public async void ReceivingCatergoriesFromCategoryTableWorks()
        {
            var all = await _cats.GetAll();
            Assert.NotNull(all);
        }

        [Fact]
        public async void ReceivingSingleFromCategoryTableWorks()
        {
            var getAll = await _cats.GetAll();
            var all = getAll as List<Categorie>;
            var rowId = all[1].Id;
            var getRow = await _cats.GetById(rowId);
            Assert.NotNull(getRow);
        }

        [Fact]
        public async void AddAndDeleteFromCategoryTableWorks()
        {
            var findRow = await _cats.Find(c => c.TypeWagen == "boot");
            _context.Categorie.RemoveRange(findRow);
            _context.SaveChanges();

            var category = new Categorie() { Id = new Guid(), TypeWagen = "boot" };
            var addRow = await _cats.Add(category);
            await _context.SaveChangesAsync();

            Assert.True(addRow);

            findRow = await _cats.Find(c => c.TypeWagen == "boot");
            var toDelete = findRow as List<Categorie>;
            var rowToDelete = toDelete[0].Id;
            var deleteRow = await _cats.Delete(toDelete[0].Id);
            await _context.SaveChangesAsync();

            findRow = await _cats.Find(c => c.TypeWagen == "boot");
            toDelete = findRow as List<Categorie>;
            Assert.Empty(toDelete);
        }

        [Fact]
        public async void ReceivingStatussesFromCategoryTableWorks()
        {
            var all = await _stats.GetAll();
            Assert.NotNull(all);
        }

        [Fact]
        public async void ReceivingSingleFromStatusTableWorks()
        {
            var getAll = await _stats.GetAll();
            var all = getAll as List<Status>;
            var rowId = all[1].Id;
            var getRow = await _stats.GetById(rowId);
            Assert.NotNull(getRow);
        }

        [Fact]
        public async void AddAndDeleteFromStatusTableShouldNotWork()
        {
            var category = new Status() { Id = new Guid(), Staat = "isboot" };
            await Assert.ThrowsAsync<NotSupportedException>(() => _stats.Add(category));

            var findRow = await _stats.Find(c => c.Staat == "aankoop");
            var toDelete = findRow as List<Status>;
            var rowToDelete = toDelete[0].Id;
            await Assert.ThrowsAsync<NotSupportedException>(() => _stats.Delete(toDelete[0].Id));
        }

        [Fact]
        public async void ReceivingBrandstoffenFromCategoryTableWorks()
        {
            var all = await _brands.GetAll();
            List<Brandstof> allEntiteiten = all as List<Brandstof>;

            Assert.NotEmpty(all);
            Assert.True(allEntiteiten.Count > 1);
        }

        [Fact]
        public async void ReceivingSingleFromBrandstofTableWorks()
        {
            var getAll = await _brands.GetAll();
            var all = getAll as List<Brandstof>;
            var rowId = all[1].Id;
            var getRow = await _brands.GetById(rowId);
            Assert.NotNull(getRow);
        }

        [Fact]
        public async void AddAndDeleteFromBrandstofTableWorks()
        {
            var findRow = await _brands.Find(c => c.TypeBrandstof == "kernfusie");
            _context.Brandstof.RemoveRange(findRow);
            _context.SaveChanges();

            var category = new Brandstof() { Id = new Guid(), TypeBrandstof = "kernfusie" };
            var addRow = await _brands.Add(category);
            await _context.SaveChangesAsync();

            Assert.True(addRow);

            findRow = await _brands.Find(c => c.TypeBrandstof == "kernfusie");
            var toDelete = findRow as List<Brandstof>;
            var rowToDelete = toDelete[0].Id;
            var deleteRow = await _brands.Delete(toDelete[0].Id);
            await _context.SaveChangesAsync();

            findRow = await _brands.Find(c => c.TypeBrandstof == "kernfusie");
            toDelete = findRow as List<Brandstof>;
            Assert.Empty(toDelete);
        }

        [Fact]
        public async void CategoryCannotBeDeletedWhenLinkedToVehicle()
        {
            var builder = GetVoertuig1();
            var voertuig = builder.Build();
            var add = await _repo.Add(voertuig);
            await _context.SaveChangesAsync();
            Assert.False(await _cats.Delete(voertuig.Categorie.Id));
            await _context.SaveChangesAsync();

            var row = await _cats.GetById(voertuig.Categorie.Id);
            Assert.NotNull(row);
        }

        [Fact]
        public async void BrandstofCannotBeDeletedWhenLinkedToVehicle()
        {
            var builder = GetVoertuig1();
            var voertuig = builder.Build();
            var add = await _repo.Add(voertuig);
            await _context.SaveChangesAsync();
            Assert.False(await _brands.Delete(voertuig.Brandstof.Id));
            await _context.SaveChangesAsync();

            var row = await _brands.GetById(voertuig.Brandstof.Id);
            Assert.NotNull(row);
        }
    }
}