using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonaKey.DataAccessLayer.Context;
using PersonaKey.DataAccessLayer.Repository.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.DataAccessLayer.Repository.Concrete
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        private readonly PersonaKeyContext _context;

        public AppUserRepository(PersonaKeyContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByUsernameWithRoleAccessAsync(string username)
        {
            return await _context.AppUsers
                .Include(u => u.Role)
                    .ThenInclude(r => r.RoleAccess)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
