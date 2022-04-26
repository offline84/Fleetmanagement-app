﻿using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL
{
    public class BrandstofRepository:GenericRepository<Brandstof>, IBrandstofRepository
    {
        private FleetmanagerContext _context;
        private readonly ILogger _logger;

        public BrandstofRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _logger = logger;
            _context = context;
        }
        public override async Task<bool> Delete(Guid id)
        {
            var voertuig = _context.Voertuigen.Where(v => v.BrandstofId == id).ToList();
            var check = voertuig.Any();
            if( check)
            {
                _logger.LogWarning("Cannot delete a fuel linked to a vehicle");
                return false;
            }

            var entity = _dbSet.Where(c => c.Id == id).FirstOrDefault();

            if (entity != null)
            {
                try
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                    var checkRow = await this.GetById(entity.Id);
                    if (checkRow == null)
                        return true;
                    else
                    {
                        _logger.LogWarning("{entity.GetType()} didn't delete", entity.GetType());
                        return false;
                    }
                }
                catch(Exception e) 
                {
                    _logger.LogError("Delete encountered an exception", e);
                    return false;
                }
            }
            _logger.LogInformation("entity was not present in database", this);
            return true;
        }
    }
}
