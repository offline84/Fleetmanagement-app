using Fleetmanagement_app_Groep1.Entities;
using FleetmanagementApp.BUL.GenericRepository;

namespace FleetmanagementApp.BUL.Repository
{
    public interface IVoertuigRepository : IGenericRepository<Voertuig>, IVoertuigBuilderRepository
    {
    }
}
