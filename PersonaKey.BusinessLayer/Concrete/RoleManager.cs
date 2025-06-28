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
    public class RoleManager : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Role role)
        {
            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role != null)
            {
                await _unitOfWork.Roles.DeleteAsync(role);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _unitOfWork.Roles.GetAllAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _unitOfWork.Roles.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Role role)
        {
            await _unitOfWork.Roles.UpdateAsync(role);
            await _unitOfWork.SaveAsync();
        }
    }
}
