using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.DataAccessLayer.UnitOfWorks.Abstract;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Concrete
{
    public class PermissionManager : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Permission permission)
        {
            await _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission != null)
            {
                await _unitOfWork.Permissions.DeleteAsync(permission);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            return await _unitOfWork.Permissions.GetAllAsync();
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            return await _unitOfWork.Permissions.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Permission permission)
        {
            await _unitOfWork.Permissions.UpdateAsync(permission);
            await _unitOfWork.SaveAsync();
        }
    }
}
