using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.DataAccessLayer.Repository.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        // List all saves
        Task<List<T>> GetAllAsync();

        // List saves with filter
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        // Get saves with Id (if Id is int or similar)
        Task<T> GetByIdAsync(int id);

        // Add new save
        Task AddAsync(T entity);

        // Update save (usually entity is sent)
        Task UpdateAsync(T entity);

        // Delete save
        Task DeleteAsync(T entity);

        // Get the only save that matches the specified filter
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
    }
}
