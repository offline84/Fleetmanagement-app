
using System.Collections.Generic;

namespace FleetmanagementApp.BUL.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Create(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}
