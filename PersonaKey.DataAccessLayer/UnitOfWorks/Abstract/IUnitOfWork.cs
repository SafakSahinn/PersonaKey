using PersonaKey.DataAccessLayer.Repository.Abstract;
using PersonaKey.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.DataAccessLayer.UnitOfWorks.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AccessLog> AccessLogs { get; }
        IGenericRepository<Card> Cards { get; }
        IGenericRepository<Department> Departments { get; }
        IGenericRepository<Door> Doors { get; }
        IGenericRepository<Permission> Permissions { get; }
        IGenericRepository<Person> Persons { get; }
        IGenericRepository<Role> Roles { get; }
        IGenericRepository<AppUser> AppUsers { get; }

        Task<int> SaveAsync();
    }
}
