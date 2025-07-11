using ItemsStoreWebAPI.Application.DTOs.Interfaces;
using ItemsStoreWebAPI.Application.Models;
using ItemsStoreWebAPI.Application.Services.Interfaces;
using ItemsStoreWebAPI.DataAccess.DataBase.Transactions.Interfaces;
using ItemsStoreWebAPI.DataAccess.Repositories.Interfaces;

namespace ItemsStoreWebAPI.Application.Services.Implementations
{
    public class MobileService(
        IStorage<Mobile> mobileStorage,
        IDbTransactionOperations<Mobile> transactionOperations) : IService<Mobile>
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
        
        public async Task<IEnumerable<Mobile>> AddMultipleAsync(IEnumerable<Mobile> mobiles)
        {
            return await transactionOperations.AddMultipleAsync(mobiles);
        }

        public async Task<IEnumerable<Mobile>> UpdateMultipleAsync(IEnumerable<Mobile> mobiles)
        {
            return await transactionOperations.UpdateMultipleAsync(mobiles);
        }

        public async Task DeleteMultipleAsync(IEnumerable<Mobile> mobiles)
        {
            await transactionOperations.DeleteMultipleAsync(mobiles);
        }
    }
}