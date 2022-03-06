using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1.Database
{
    public class FleetManagerDbConfig: DbConfiguration
    {
        public FleetManagerDbConfig()
        {
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
        }
    }
}
