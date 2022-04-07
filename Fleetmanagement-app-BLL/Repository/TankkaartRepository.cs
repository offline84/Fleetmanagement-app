using Fleetmanagement_app_Groep1.Database;
using Fleetmanagement_app_Groep1.Entities;
using Fleetmanagement_app_BLL.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public class TankkaartRepository : GenericRepository<Tankkaart>, ITankkaartRepository
    {
        public TankkaartRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        public override async Task<bool> Add(Tankkaart tankkaart)
        {
            List<ToewijzingBrandstofTankkaart> toewijzingbrandstof = new List<ToewijzingBrandstofTankkaart>();
            tankkaart.Kaartnummer = tankkaart.Kaartnummer.Trim();

            if (tankkaart.GeldigheidsDatum == null | tankkaart.Kaartnummer == "")
            {
                return false;
            }

            await _context.ToewijzingBrandstofTankkaarten.AddRangeAsync(toewijzingbrandstof);
            await _dbSet.AddAsync(tankkaart);

            return true;
        }

        public override async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();      
        }

        public override async Task<IEnumerable<Tankkaart>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override async Task<Tankkaart> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> Update(Tankkaart tankkaart)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Tankkaart>> Find(Expression<Func<Tankkaart, bool>> predicate)
        {
            throw new NotImplementedException();
        }

    }

}
