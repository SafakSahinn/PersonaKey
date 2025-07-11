using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.DataAccessLayer.Repository.Abstract
{
    public interface IAppUserRepository : IGenericRepository<AppUser>
    {
        Task<AppUser?> GetByUsernameWithRoleAccessAsync(string username);
    }
}
