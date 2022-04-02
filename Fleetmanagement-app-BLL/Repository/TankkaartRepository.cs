using Fleetmanagement_app_Groep1.Database;
using Fleetmanagement_app_Groep1.Entities;
using Fleetmanagement_app_BLL.GenericRepository;
using Microsoft.Extensions.Logging;

namespace Fleetmanagement_app_BLL.Repository
{
    public class TankkaartRepository : GenericRepository<Tankkaart>, ITankkaartRepository
    {
        public TankkaartRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {

        }
    }
}
