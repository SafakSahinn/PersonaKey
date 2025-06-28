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
    public class DoorManager : IDoorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoorManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Door door)
        {
            await _unitOfWork.Doors.AddAsync(door);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var door = await _unitOfWork.Doors.GetByIdAsync(id);
            if (door != null)
            {
                await _unitOfWork.Doors.DeleteAsync(door);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<Door>> GetAllAsync()
        {
            return await _unitOfWork.Doors.GetAllAsync();
        }

        public async Task<Door> GetByIdAsync(int id)
        {
            return await _unitOfWork.Doors.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Door door)
        {
            await _unitOfWork.Doors.UpdateAsync(door);
            await _unitOfWork.SaveAsync();
        }
    }
}
