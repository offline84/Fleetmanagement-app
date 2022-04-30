using Fleetmanagement_app_BLL.Repository;
using Fleetmanagement_app_DAL.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FleetmanagerContext _context;
        private readonly ILogger _logger;

        public IBestuurderRepository Bestuurder { get; private set; }
        public IVoertuigRepository Voertuig { get; private set; }
        public ITankkaartRepository Tankkaart { get; private set; }
        public IKoppelingRepository Koppeling { get; private set; }

        public IStatusRepository Status { get; private set; }
        public IBrandstofRepository Brandstof { get; private set; }
        public ICategorieRepository Categorie { get; private set; }
        public IRijbewijsRepository Rijbewijs { get; private set; }

        public UnitOfWork(FleetmanagerContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Bestuurder = new BestuurderRepository(context, _logger);
            Voertuig = new VoertuigRepository(context, _logger);
            Tankkaart = new TankkaartRepository(context, _logger);
            Koppeling = new KoppelingRepository(context, _logger);

            Status = new StatusRepository(context, _logger);
            Brandstof = new BrandstofRepository(context, _logger);
            Categorie = new CategorieRepository(context, _logger);
            Rijbewijs = new RijbewijsRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}