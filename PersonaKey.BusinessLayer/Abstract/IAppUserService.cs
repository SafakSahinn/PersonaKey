using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Abstract
{
    public interface IAppUserService
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser> GetByIdAsync(int id);
        Task AddAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task DeleteAsync(int id);
        Task<AppUser> GetByUsernameAsync(string username);
    }
}
