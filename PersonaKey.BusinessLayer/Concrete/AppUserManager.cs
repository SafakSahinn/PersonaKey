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
    public class AppUserManager : IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppUserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AppUser user)
        {
            await _unitOfWork.AppUsers.AddAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _unitOfWork.AppUsers.GetByIdAsync(id);
            if (user != null)
            {
                await _unitOfWork.AppUsers.DeleteAsync(user);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _unitOfWork.AppUsers.GetAllAsync();
        }

        public async Task<AppUser> GetByIdAsync(int id)
        {
            return await _unitOfWork.AppUsers.GetByIdAsync(id);
        }

        public async Task<AppUser> GetByUsernameAsync(string username)
        {
            var users = await _unitOfWork.AppUsers.GetAllAsync();
            return users.FirstOrDefault(u => u.UserName == username);
        }

        public async Task UpdateAsync(AppUser user)
        {
            await _unitOfWork.AppUsers.UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<AppUser?> GetByUsernameWithRoleAccessAsync(string username)
        {
            return await _unitOfWork.AppUsers.GetByUsernameWithRoleAccessAsync(username);
        }
    }
}
