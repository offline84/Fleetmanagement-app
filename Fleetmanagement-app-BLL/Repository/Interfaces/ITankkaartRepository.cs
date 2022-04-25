using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface ITankkaartRepository : IGenericRepository<Tankkaart>
    {
        Task<bool> Blokkeren(string kaartnummer);
    }
}