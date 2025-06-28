using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Abstract
{
    public interface IDoorService
    {
        Task<List<Door>> GetAllAsync();
        Task<Door> GetByIdAsync(int id);
        Task AddAsync(Door door);
        Task UpdateAsync(Door door);
        Task DeleteAsync(int id);
    }
}
