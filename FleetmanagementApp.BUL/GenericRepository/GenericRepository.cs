using Fleetmanagement_app_Groep1.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FleetmanagementApp.BUL.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private FleetmanagerContext _context = null;
        private DbSet<T> dbSet = null;

        public GenericRepository(FleetmanagerContext fleetmanagerContext)
        {
            _context = fleetmanagerContext;
            dbSet = _context.Set<T>();
        }

        public void Create(T obj)
        {
            dbSet.Add(obj);
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(object id)
        {
            return dbSet.Find(id);
        }

        public void Update(T obj)
        {
            dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
