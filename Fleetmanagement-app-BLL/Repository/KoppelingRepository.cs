using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
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