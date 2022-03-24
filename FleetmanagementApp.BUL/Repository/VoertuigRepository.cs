using Fleetmanagement_app_Groep1.Database;
using FleetmanagementApp.BUL.GenericRepository;
using FleetmanagementApp.BUL.Models;
using Microsoft.Extensions.Logging;

namespace FleetmanagementApp.BUL.Repository
{
    public class VoertuigRepository : GenericRepository<VoertuigModel>, IVoertuigRepository
    {
        public VoertuigRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {

        }
    }
}
