using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Entities;
using System.Collections.Generic;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface IBestuurderRepository : IGenericRepository<Bestuurder>
    {
        List<Rijbewijs> GetDriverLicensesForDriver(string rijksregisternummer);
    }
}