using PersonaKey.DataAccessLayer.Context;
using PersonaKey.DataAccessLayer.Repository.Abstract;
using PersonaKey.DataAccessLayer.Repository.Concrete;
using PersonaKey.DataAccessLayer.UnitOfWorks.Abstract;
using PersonaKey.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaKey.DataAccessLayer.UnitOfWorks.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PersonaKeyContext _context;
        private bool _disposed = false;

        // Constructor
        public UnitOfWork(PersonaKeyContext context)
        {
            _context = context;
        }

        // Lazy loading: Repository instance is created only when first accessed
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
                    // Release managed resources
                    _context.Dispose();
                }

                // If there are no unmanaged resources you don't need to do anything
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Don't let the garbage collector call the finalizer
        }
    }
}