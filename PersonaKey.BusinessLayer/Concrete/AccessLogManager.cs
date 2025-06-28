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
    public class AccessLogManager : IAccessLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccessLogManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AccessLog accessLog)
        {
            await _unitOfWork.AccessLogs.AddAsync(accessLog);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _unitOfWork.AccessLogs.GetByIdAsync(id);
            if (log != null)
            {
                await _unitOfWork.AccessLogs.DeleteAsync(log);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<List<AccessLog>> GetAllAsync()
        {
            return await _unitOfWork.AccessLogs.GetAllAsync();
        }

        public async Task<AccessLog> GetByIdAsync(int id)
        {
            return await _unitOfWork.AccessLogs.GetByIdAsync(id);
        }

        public async Task UpdateAsync(AccessLog accessLog)
        {
            await _unitOfWork.AccessLogs.UpdateAsync(accessLog);
            await _unitOfWork.SaveAsync();
        }
    }
}
