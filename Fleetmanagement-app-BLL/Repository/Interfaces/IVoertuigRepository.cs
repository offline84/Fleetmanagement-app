using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_BLL.GenericRepository;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface IVoertuigRepository : IGenericRepository<Voertuig>, IVoertuigBuilderRepository
    {
    }
}
