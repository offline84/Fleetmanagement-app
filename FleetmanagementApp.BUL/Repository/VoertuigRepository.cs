using Fleetmanagement_app_DAL.Builders;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using FleetmanagementApp.BUL.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FleetmanagementApp.BUL.Repository
{
    public class VoertuigRepository : GenericRepository<Voertuig>, IVoertuigRepository
    {
        private FleetmanagerContext _context;
        private readonly ILogger _logger;
        private DbSet<Voertuig> _dbSet;
        public VoertuigRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _logger = logger;
            _context = context;
            _dbSet = _context.Voertuigen;
        }
          
        /// <summary>
        ///     Add() voegt het voertuig toe aan de database. 
        /// </summary>
        /// <remarks>
        ///     Let wel: voertuig dient eerst gebouwd te worden via de buildersklasse vooraleer mee te geven aan de method.
        /// </remarks>
        /// <param name="voertuig"></param>
        /// <returns>boolean</returns>
        /// 
        public override async Task<bool> Add(Voertuig voertuig)
        {
            try
            {

                if(_dbSet.Where(v => v.Chassisnummer == voertuig.Chassisnummer).Any())
                {
                   _logger.LogWarning("De database bevat reeds een voertuig met hetzelfde chassisnummer!", voertuig.Chassisnummer);
                    return false;
                }
               
                if(voertuig.Nummerplaat.Trim() != "")
                {
                    if(_dbSet.Where(v => v.Nummerplaat.ToUpper() == voertuig.Nummerplaat.ToUpper()).Any())
                    {
                        _logger.LogWarning("De database bevat reeds een voertuig met dezelfde nummerplaat!", voertuig.Nummerplaat);
                        return false;
                    }
                }
               

                voertuig.LaatstGeupdate =DateTime.Now;
                await _dbSet.AddAsync(voertuig);
                return true;
            }
            catch(Exception e)
            {
                _logger.LogWarning(e, e.Message);
                return false;
            }
        }

        /// <summary>
        ///     Overload van Delete method van de hoofdklassen "id = string"
        /// </summary>
        /// <param string ="chassisnummer"></param>
        /// <returns> boolean </returns>

        public override async Task<bool> Delete(string chassisnummer)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Voertuig>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public override async Task<Voertuig> GetById(string chassisnummer)
        {
            return await _dbSet.FindAsync(chassisnummer);
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
