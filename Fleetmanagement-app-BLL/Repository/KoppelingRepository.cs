using Fleetmanagement_app_Groep1.Database;
using Fleetmanagement_app_Groep1.Entities;
using Fleetmanagement_app_BLL.GenericRepository;
using Microsoft.Extensions.Logging;

namespace Fleetmanagement_app_BLL.Repository
{
    public class KoppelingRepository : GenericRepository<Koppeling>, IKoppelingRepository
    {
        public KoppelingRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {

        }
    }
}
