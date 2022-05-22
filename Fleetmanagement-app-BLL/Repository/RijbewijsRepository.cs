using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL
{
    public class RijbewijsRepository : GenericRepository<Rijbewijs>, IRijbewijsRepository
    {
        public RijbewijsRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        /// <summary>
        /// Delete for Rijbewijs.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> Delete(Guid id)
        {
           throw new NotSupportedException("Delete is not allowed on the rijbewijs table!");
        }

        /// <summary>
        /// Geeft alle Rijbewijss terug.
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Rijbewijs>> GetAll()
        {
            return await _dbSet
                .Include(t=> t.ToewijzingenBestuurder)
                .ToListAsync();
        }
    }
}