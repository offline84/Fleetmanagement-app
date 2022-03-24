using Fleetmanagement_app_Groep1.Database;
using FleetmanagementApp.BUL.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Fleetmanagement_app_Groep1.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using FleetmanagementApp.BUL.GenericRepository;

namespace FleetmanagementApp.BUL.Repository
{
    public class BestuurderRepository : GenericRepository<BestuurderModel>, IBestuurderRepository
    {
        private Mapper _bestuurderMapper;
        private FleetmanagerContext _context;
        public BestuurderRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
            var _configBestuurder = new MapperConfiguration(cfg => cfg.CreateMap<Bestuurder, BestuurderModel>());
            _bestuurderMapper = new Mapper(_configBestuurder);
        }

        public override async Task<bool> Add(BestuurderModel model)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<BestuurderModel>> GetAll()
        {
            var entiteiten = await _context.Bestuurders.ToListAsync();
            var models = _bestuurderMapper.Map<List<Bestuurder>, List<BestuurderModel>>(entiteiten);
            return models;
        }

        public override async Task<BestuurderModel> GetById(Guid id)
        {
            var entiteit = await _context.Bestuurders.FindAsync(id);
            var model = _bestuurderMapper.Map<Bestuurder, BestuurderModel>(entiteit);
            return model;
        }

        public virtual Task<bool> Update(BestuurderModel model)
        {
            throw new NotImplementedException();
        }
    }
}
