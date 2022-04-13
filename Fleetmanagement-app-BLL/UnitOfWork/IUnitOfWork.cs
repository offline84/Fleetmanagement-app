using Fleetmanagement_app_BLL.Repository;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBestuurderRepository Bestuurder { get; }
        IVoertuigRepository Voertuig { get; }
        ITankkaartRepository Tankkaart { get; }
        IKoppelingRepository Koppeling { get; }

        Task CompleteAsync();

        void Dispose();
    }
}