using PersonaKey.DataAccessLayer.Context;
using PersonaKey.DataAccessLayer.Repository.Abstract;
using PersonaKey.DataAccessLayer.Repository.Concrete;
using PersonaKey.DataAccessLayer.UnitOfWorks.Abstract;
using PersonaKey.EntityLayer.Concrete;
using System;
using System.Threading.Tasks;

namespace PersonaKey.DataAccessLayer.UnitOfWorks.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PersonaKeyContext _context;
        private bool _disposed = false;

        public UnitOfWork(PersonaKeyContext context)
        {
            _context = context;
        }

        private IGenericRepository<AccessLog> _accessLogs;
        public IGenericRepository<AccessLog> AccessLogs => _accessLogs ??= new GenericRepository<AccessLog>(_context);

        private IGenericRepository<Card> _cards;
        public IGenericRepository<Card> Cards => _cards ??= new GenericRepository<Card>(_context);

        private IGenericRepository<Department> _departments;
        public IGenericRepository<Department> Departments => _departments ??= new GenericRepository<Department>(_context);

        private IGenericRepository<Door> _doors;
        public IGenericRepository<Door> Doors => _doors ??= new GenericRepository<Door>(_context);

        private IGenericRepository<Permission> _permissions;
        public IGenericRepository<Permission> Permissions => _permissions ??= new GenericRepository<Permission>(_context);

        private IGenericRepository<Person> _persons;
        public IGenericRepository<Person> Persons => _persons ??= new GenericRepository<Person>(_context);

        private IGenericRepository<Role> _roles;
        public IGenericRepository<Role> Roles => _roles ??= new GenericRepository<Role>(_context);

        // ÖZEL: AppUser için özel repository kullanımı
        private IAppUserRepository _appUsers;
        public IAppUserRepository AppUsers => _appUsers ??= new AppUserRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
