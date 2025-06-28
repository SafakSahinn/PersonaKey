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
    public class CardManager : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CardManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Card card)
        {
            await _unitOfWork.Cards.AddAsync(card);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var card = await _unitOfWork.Cards.GetByIdAsync(id);
            if (card != null)
            {
                await _unitOfWork.Cards.DeleteAsync(card);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<Card>> GetAllAsync()
        {
            return await _unitOfWork.Cards.GetAllAsync();
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            return await _unitOfWork.Cards.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Card card)
        {
            await _unitOfWork.Cards.UpdateAsync(card);
            await _unitOfWork.SaveAsync();
        }
    }
}
