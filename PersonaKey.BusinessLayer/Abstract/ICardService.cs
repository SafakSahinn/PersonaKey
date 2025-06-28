using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonaKey.EntityLayer.Concrete;

namespace PersonaKey.BusinessLayer.Abstract
{
    public interface ICardService
    {
        Task<List<Card>> GetAllAsync();
        Task<Card> GetByIdAsync(int id);
        Task AddAsync(Card card);
        Task UpdateAsync(Card card);
        Task DeleteAsync(int id);
    }
}
