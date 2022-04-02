using Fleetmanagement_app_Groep1.Database;
using Fleetmanagement_app_Groep1.Helpers;
using FleetmanagementApp.BUL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class VoertuigRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions());
        private static readonly ILogger _logger;
        private VoertuigRepository _repo = new VoertuigRepository(_context, _logger);

        [Fact]
        public void EenChassisnummerMagMaarAan1WagenToegekendZijn()
        {
           
        }

        [Fact]
        public void EenNummerplaatMagMaarAan1WagenToegekendZijn()
        {

        }
    }
}
