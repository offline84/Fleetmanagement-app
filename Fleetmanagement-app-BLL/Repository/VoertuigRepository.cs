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
    public class VoertuigRepository : GenericRepository<Voertuig>, IVoertuigRepository
    {
        public VoertuigRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        { }
           
        public override async Task<bool> Add(Voertuig voertuig)
        {
            if (RequiredPropertiesCheck(voertuig))
            {
                await _dbSet.AddAsync(voertuig);
                return true;
            }

            return false;
        }


        //Overload van Delete method van de hoofdklassen "id = string"
        public override async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Voertuig>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public override async Task<Voertuig> GetById(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public override Task<bool> Update(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Voertuig>> Find(Expression<Func<Voertuig, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public bool RequiredPropertiesCheck(Voertuig voertuig)
        {
            voertuig.Chassisnummer = voertuig.Chassisnummer.Trim().ToUpper();
            voertuig.Merk = voertuig.Merk.Trim();
            voertuig.Model = voertuig.Model.Trim();

            if(voertuig.Chassisnummer == ""| voertuig.Merk == "" | voertuig.Model == "")
                return false;
            
            return true;
        }

        public void New(Voertuig voertuig)
        {
            throw new NotImplementedException();
        }
    }
}
