using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Entities;
using FleetmanagementApp.BUL.GenericRepository;

namespace FleetmanagementApp.BUL.Repository
{
    public interface IVoertuigRepository : IGenericRepository<Voertuig>, IVoertuigBuilderRepository
    {
    }
}
