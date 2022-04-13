using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public class BestuurderRepository : GenericRepository<Bestuurder>, IBestuurderRepository
    {
        private FleetmanagerContext _context;

        public BestuurderRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<bool> Add(Bestuurder bestuurder)
        {
            List<ToewijzingRijbewijsBestuurder> toewijzingen = new List<ToewijzingRijbewijsBestuurder>();
            bestuurder.Naam = bestuurder.Naam.Trim();
            bestuurder.Achternaam = bestuurder.Achternaam.Trim();
            bestuurder.Rijksregisternummer = bestuurder.Rijksregisternummer.Trim();

            if (bestuurder.Rijksregisternummer == "" |
                bestuurder.Naam == "" |
                bestuurder.Achternaam == "" |
                bestuurder.GeboorteDatum == null |
                bestuurder.Rijbewijzen.Count == 0)

                return false;

            foreach (var rijbewijs in bestuurder.Rijbewijzen)
            {
                ToewijzingRijbewijsBestuurder trb = new ToewijzingRijbewijsBestuurder();
                trb.Rijbewijs = rijbewijs;
                trb.Bestuurder = bestuurder;

                toewijzingen.Add(trb);
            }

            await _context.ToewijzingRijbewijsBestuurders.AddRangeAsync(toewijzingen);
            await _dbSet.AddAsync(bestuurder);

            return true;
        }

        public override async Task<IEnumerable<Bestuurder>> GetAll()
        {
            var entiteiten = await _context.Bestuurders.ToListAsync<Bestuurder>();
            return entiteiten;
        }

        public override async Task<Bestuurder> GetById(string id)
        {
            var bestuurder = await _context.Bestuurders.FindAsync(id);
            return bestuurder;
        }

        public List<Rijbewijs> GetDriverLicensesForDriver(string rijksregisternummer)
        {
            throw new NotImplementedException();
        }
    }
}