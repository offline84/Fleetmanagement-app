using Fleetmanagement_app_Groep1.Entities;
using FleetmanagementApp.BUL.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FleetmanagementApp.BUL.Repository
{
    public interface IBestuurderRepository : IGenericRepository<Bestuurder>
    {
        List<Rijbewijs> GetDriverLicensesForDriver(string rijksregisternummer);
    }
}
