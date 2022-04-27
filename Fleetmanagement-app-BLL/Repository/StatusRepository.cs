using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL
{
    public class StatusRepository:GenericRepository<Status>, IStatusRepository
    {
        /// <summary>
        ///     Status is read- only. Delete en Add zijn hierdoor not supported in deze repository.
        /// </summary>

        private FleetmanagerContext _context;
        private readonly ILogger _logger;

        public StatusRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _logger = logger;
            _context = context;
        }
        public override async Task<bool> Delete(Guid id)
        {
            throw new NotSupportedException("Delete is not allowed on the status table!");
        }

        public override Task<bool> Add(Status entity)
        {
            throw new NotSupportedException("Add is not allowed on the status table!");
        }
    }
}
