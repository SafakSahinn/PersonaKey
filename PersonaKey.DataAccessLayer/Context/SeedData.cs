using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.DataAccessLayer.Context
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new PersonaKeyContext(
                serviceProvider.GetRequiredService<DbContextOptions<PersonaKeyContext>>());

            if (context.Roles.Any())
                return; // Daha önce eklenmişse tekrar ekleme

            var adminRole = new Role
            {
                Name = "Admin",
                RoleAccess = new RoleAccess
                {
                    CanLogin = true,
                    CanEditSite = true
                }
            };

            var userRole = new Role
            {
                Name = "User",
                RoleAccess = new RoleAccess
                {
                    CanLogin = true,
                    CanEditSite = false
                }
            };

            context.Roles.AddRange(adminRole, userRole);
            context.SaveChanges();
        }
    }
}
