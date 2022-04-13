using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using FleetmanagementApp.BUL.GenericRepository;
using Microsoft.Extensions.Logging;

namespace FleetmanagementApp.BUL.Repository
{
    public class KoppelingRepository : GenericRepository<Koppeling>, IKoppelingRepository
    {
        public KoppelingRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {

        }
    }
}
