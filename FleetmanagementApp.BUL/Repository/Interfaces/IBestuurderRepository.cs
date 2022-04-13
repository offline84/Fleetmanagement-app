using Fleetmanagement_app_DAL.Entities;
using FleetmanagementApp.BUL.GenericRepository;
using System.Collections.Generic;

namespace FleetmanagementApp.BUL.Repository
{
    public interface IBestuurderRepository : IGenericRepository<Bestuurder>
    {
        List<Rijbewijs> GetDriverLicensesForDriver(string rijksregisternummer);
    }
}
