using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;

namespace ItemsStoreWebAPI.Services
{
    public class TvService(IStorageBase<TV> tvStorage) : IServiceBase<TV>
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
    }
}