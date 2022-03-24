using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FleetmanagementApp.BUL.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}
