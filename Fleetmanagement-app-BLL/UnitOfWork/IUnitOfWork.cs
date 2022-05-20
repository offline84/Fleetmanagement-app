using Fleetmanagement_app_BLL.Repository;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBestuurderRepository Bestuurder { get; }
        IVoertuigRepository Voertuig { get; }
        ITankkaartRepository Tankkaart { get; }
        IBrandstofRepository Brandstof { get; }
        ICategorieRepository Categorie { get; }
        IStatusRepository Status { get; }
        IKoppelingRepository Koppeling { get; }
        IRijbewijsRepository Rijbewijs { get; }

        Task CompleteAsync();

        void Dispose();
    }
}