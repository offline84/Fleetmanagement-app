using Fleetmanagement_app_Groep1.Database;
using Fleetmanagement_app_Groep1.Helpers;
using Fleetmanagement_app_BLL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fleetmanagement_Unit_Tests
{
    public class TankkaartRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions());
        private static readonly ILogger _logger;
        private TankkaartRepository _TankkaartRepository = new TankkaartRepository(_context, _logger);



    }
}
