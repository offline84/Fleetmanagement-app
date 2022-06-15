using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface IBestuurderRepository : IGenericRepository<Bestuurder>
    {
        Task<Bestuurder> GetByIdNoTracking(string id);
    }
}