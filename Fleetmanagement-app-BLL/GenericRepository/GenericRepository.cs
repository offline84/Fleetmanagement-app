using Fleetmanagement_app_DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected FleetmanagerContext _context;
        internal DbSet<T> _dbSet;
        private FleetmanagerContext context;
        private ILogger logger;
        protected readonly ILogger _logger;

        public GenericRepository(FleetmanagerContext context, ILogger logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }


        public GenericRepository(FleetmanagerContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        //Overload van Delete method van de hoofdklassen "id = string"
        public virtual async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        //Overload van GetbyId method van de hoofdklassen "id = string"
        public virtual async Task<T> GetById(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual Task<bool> Update(T obj)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}