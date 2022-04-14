using Fleetmanagement_app_Groep1.Database;
using Fleetmanagement_app_Groep1.Helpers;
using Fleetmanagement_app_BLL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Fleetmanagement_app_Groep1.Entities;
using System.Threading.Tasks;

namespace Fleetmanagement_Unit_Tests
{
    public class TankkaartRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions());
        private ILoggerFactory _loggerfactory = new LoggerFactory();
        //public ILogger _logger;
        private TankkaartRepository _repo; // = new TankkaartRepository(_context, _logger);

        public TankkaartRepositoryTests()
        {
            ILoggerFactory loggerfactory = new LoggerFactory();
            _repo = new TankkaartRepository(_context, _loggerfactory.CreateLogger("Tankkartlogs"));
        }

        internal Tankkaart TestCreateTankkaart(string kaartnummer, DateTime geldigheidsDatum, int pincode)
        {
            var tankkaart = new Tankkaart(kaartnummer, geldigheidsDatum);
            if (pincode > 0) { tankkaart.Pincode = pincode; }
            return tankkaart;
        }

        [Theory]
        [InlineData("123456789","11/04/2022",12345)]
        [InlineData("15645", "21/12/2021", 123456)]
        [InlineData("98jhjg84", "01/01/2020", 954865)]
        public async Task AddTankkaart_DbNotEmptyAsync(string kaartnummer, string geldigheidsDatumString, int pincode)
        {
            DateTime geldigheidsDatum = DateTime.Parse(geldigheidsDatumString);
            var tankkaart = TestCreateTankkaart(kaartnummer, geldigheidsDatum, pincode);

            Assert.NotNull(tankkaart);
            Assert.True(await _repo.Add(tankkaart));
        }

    }
}
