using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Entities;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface IKoppelingRepository : IGenericRepository<Koppeling>
    {
        Task<bool> KoppelAanTankkaart(string bestuurderRRN, string kaartnummer);
        Task<bool> KoppelLosVanTankkaart(string kaartnummer);
        Task<bool> KoppelAanVoertuig(string bestuurderRRN, string chassisnummer);
        Task<bool> KoppelLosVanVoertuig(string chassisnummer);
        Task<Koppeling> GetByBestuurder(string bestuurderRRN);
        Task<Koppeling> GetByTankkaart(string kaartnummer);
        Task<Koppeling> GetByvoertuig(string chassisnummer);
    }
}