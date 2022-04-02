using Fleetmanagement_app_Groep1.Entities;
using Fleetmanagement_app_BLL.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface IBestuurderRepository : IGenericRepository<Bestuurder>
    {
        List<Rijbewijs> GetDriverLicensesForDriver(string rijksregisternummer);
    }
}
