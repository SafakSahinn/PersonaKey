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
    public class PersonManager : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Person person)
        {
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var person = await _unitOfWork.Persons.GetByIdAsync(id);
            if (person != null)
            {
                await _unitOfWork.Persons.DeleteAsync(person);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<Person>> GetAllAsync()
        {
            return await _unitOfWork.Persons.GetAllAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _unitOfWork.Persons.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Person person)
        {
            await _unitOfWork.Persons.UpdateAsync(person);
            await _unitOfWork.SaveAsync();
        }
    }
}
