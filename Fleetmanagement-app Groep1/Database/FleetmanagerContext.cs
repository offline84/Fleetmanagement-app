using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Fleetmanagement_app_Groep1.Database
{
    [DbConfigurationType(typeof(FleetManagerDbConfig))]
    public class FleetmanagerContext : DbContext
    {
        private const string ConnectionString =
            @"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=FleetManagerdb;Integrated Security=true;";
                
                
        public FleetmanagerContext(DbContextOptions<FleetmanagerContext> options ) : base(options)
        {
        }
    }
}