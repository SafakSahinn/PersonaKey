using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Abstract
{
    public interface IAccessLogService
    {
        Task<List<AccessLog>> GetAllAsync();
        Task<AccessLog> GetByIdAsync(int id);
        Task AddAsync(AccessLog accessLog);
        Task UpdateAsync(AccessLog accessLog);
        Task DeleteAsync(int id);
    }
}
