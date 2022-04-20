using Fleetmanagement_app_BLL;
using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class SeededClassesRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
        private CategorieRepository _cats;
        private StatusRepository _stats;
        private BrandstofRepository _brands;
        private ILoggerFactory _loggerFactory = new LoggerFactory();

        public SeededClassesRepositoryTests()
        {
            _cats = new CategorieRepository(_context, _loggerFactory.CreateLogger("seededCategoryTest"));
            _stats = new StatusRepository(_context, _loggerFactory.CreateLogger("seededStatusTest"));
            _brands = new BrandstofRepository(_context, _loggerFactory.CreateLogger("seededBrandstofTest"));
            
            if (!_context.Database.CanConnect())
            {
                _context.Database.EnsureCreated();
                _context.Database.Migrate();
            }
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

            var category = new Categorie(){Id = new Guid(), TypeWagen = "boot"};
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

            var category = new Status(){Id = new Guid(), Staat = "isboot"};
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

            var category = new Brandstof(){Id = new Guid(), TypeBrandstof = "kernfusie"};
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
    }
}
