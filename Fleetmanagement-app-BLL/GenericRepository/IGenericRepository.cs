using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(Guid id);

        Task<T> GetById(string id);

        Task<bool> Add(T entity);

        Task<bool> Update(T entity);

        Task<bool> Delete(Guid id);

        Task<bool> Delete(string id);

        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}