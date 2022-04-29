using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Entities;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface IVoertuigRepository : IGenericRepository<Voertuig>, IVoertuigBuilderRepository
    {
        bool RequiredPropertiesCheck(Voertuig voertuig);
    }
}