using FleetmanagementApp.BUL.Repository;
using System.Threading.Tasks;

namespace FleetmanagementApp.BUL.UnitOfWork
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
