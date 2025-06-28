using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Abstract
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}
