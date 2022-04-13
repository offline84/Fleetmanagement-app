using Fleetmanagement_app_DAL.Database;
using FleetmanagementApp.BUL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FleetmanagementApp.BUL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FleetmanagerContext _context;
        private readonly ILogger _logger;

        public IBestuurderRepository Bestuurder { get; private set; }
        public IVoertuigRepository Voertuig { get; private set; }
        public ITankkaartRepository Tankkaart { get; private set; }
        public IKoppelingRepository Koppeling { get; private set; }

        public UnitOfWork(FleetmanagerContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Bestuurder = new BestuurderRepository(context, _logger);
            Voertuig = new VoertuigRepository(context, _logger);
            Tankkaart = new TankkaartRepository(context, _logger);
            Koppeling = new KoppelingRepository(context, _logger);
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
