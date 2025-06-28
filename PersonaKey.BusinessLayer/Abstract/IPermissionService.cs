using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Abstract
{
    public interface IPermissionService
    {
        Task<List<Permission>> GetAllAsync();
        Task<Permission> GetByIdAsync(int id);
        Task AddAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(int id);
    }
}
