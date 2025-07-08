using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;

namespace ItemsStoreWebAPI.Services
{
    public class MobileService(IStorage<Mobile> mobileStorage) : IService<Mobile>
    {
        public async Task<Mobile?> AddAsync(Mobile mobile)
        {
            return await mobileStorage.AddAsync(mobile);
        }

        public async Task<Mobile?> GetByIdAsync(int id)
        {
            return await mobileStorage.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Mobile>> GetAllAsync(IFilterDto? filter = null)
        {
            return await mobileStorage.GetAllAsync(filter);
        }

        public async Task<Mobile?> UpdateAsync(int id, Mobile updatedMobile)
        {
            return await mobileStorage.UpdateAsync(id, updatedMobile);
        }

        public async Task DeleteAsync(int id)
        {
            await mobileStorage.DeleteAsync(id);
        }
    }
}