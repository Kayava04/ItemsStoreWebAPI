using ItemsStoreWebAPI.Application.DTOs.Interfaces;
using ItemsStoreWebAPI.Application.Models;
using ItemsStoreWebAPI.Application.Services.Interfaces;
using ItemsStoreWebAPI.DataAccess.DataBase.Transactions.Interfaces;
using ItemsStoreWebAPI.DataAccess.Repositories.Interfaces;

namespace ItemsStoreWebAPI.Application.Services.Implementations
{
    public class TvService(
        IStorage<TV> tvStorage,
        IDbTransactionOperations<TV> transactionOperations) : IService<TV>
    {
        public async Task<TV?> AddAsync(TV tv)
        {
            return await tvStorage.AddAsync(tv);
        }

        public async Task<TV?> GetByIdAsync(int id)
        {
            return await tvStorage.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TV>> GetAllAsync(IFilterDto? filter = null)
        {
            return await tvStorage.GetAllAsync(filter);
        }

        public async Task<TV?> UpdateAsync(int id, TV updatedTv)
        {
            return await tvStorage.UpdateAsync(id, updatedTv);
        }

        public async Task DeleteAsync(int id)
        {
            await tvStorage.DeleteAsync(id);
        }
        
        public async Task<IEnumerable<TV>> AddMultipleAsync(IEnumerable<TV> tvs)
        {
            return await transactionOperations.AddMultipleAsync(tvs);
        }

        public async Task<IEnumerable<TV>> UpdateMultipleAsync(IEnumerable<TV> tvs)
        {
            return await transactionOperations.UpdateMultipleAsync(tvs);
        }

        public async Task DeleteMultipleAsync(IEnumerable<TV> tvs)
        {
            await transactionOperations.DeleteMultipleAsync(tvs);
        }
    }
}