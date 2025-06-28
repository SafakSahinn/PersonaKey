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
    public class DepartmentManager : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Department department)
        {
            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department != null)
            {
                await _unitOfWork.Departments.DeleteAsync(department);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _unitOfWork.Departments.GetAllAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _unitOfWork.Departments.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Department department)
        {
            await _unitOfWork.Departments.UpdateAsync(department);
            await _unitOfWork.SaveAsync();
        }
    }
}
