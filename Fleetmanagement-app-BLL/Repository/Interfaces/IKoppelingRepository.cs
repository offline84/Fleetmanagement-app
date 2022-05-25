using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Entities;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public interface IKoppelingRepository : IGenericRepository<Koppeling>
    {
        Task<bool> CreateKoppeling(string bestuurderRRN);
        Task<bool> KoppelBestuurderEnTankkaart(string bestuurderRRN, string kaartnummer);
        Task<bool> KoppelBestuurderEnVoertuig(string bestuurderRRN, string chassisnummer);
        Task<bool> KoppelLosTankkaart(string kaartnummer);
        Task<bool> KoppelLosVoertuig(string chassisnummer);
        Task<bool> KoppelLosBestuurder(string bestuurderRRN);
        Task<Koppeling> GetByBestuurder(string bestuurderRRN);
        Task<Koppeling> GetByTankkaart(string kaartnummer);
        Task<Koppeling> GetByvoertuig(string chassisnummer);
        bool BestuurderAlGekoppeldAanEenTankkaart(string bestuurderRRN);
        bool BestuurderAlGekoppeldAanEenVoertuig(string bestuurderRRN);
        bool TankkaartAlGekoppeldAanAndereBestuurder(string bestuurderRRN, string tankkaartnummer);
        bool VoertuigAlGekoppeldAanAndereBestuurder(string bestuurderRRN, string chassisnummer);
        bool KoppelingMetTankkaartBestaatNiet(string kaartnummer);
        bool KoppelingMetVoertuigBestaatNiet(string chassisnummer);
    }
}