using AutoMapper;
using ItemsStoreWebAPI.DataBase;
using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.DTOs.Mobile;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Extensions;
using ItemsStoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Repositories
{
    public class MobileDbStorage(
        BaseDbContext context,
        IMapper mapper,
        ILogger<MobileDbStorage> logger) : IStorageBase<Mobile>
    {
        public async Task<Mobile?> AddAsync(Mobile mobile)
        {
            var mobileEntity = mapper.Map<MobileEntity>(mobile);
            
            await context.Mobiles.AddAsync(mobileEntity);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Added Mobile with ID: {mobile.Id}.");
            return mapper.Map<Mobile>(mobileEntity);
        }

        public async Task<Mobile?> GetByIdAsync(int id)
        {
            var mobileEntity = await context.Mobiles
                .AsNoTracking()
                .Include(m => m.StockItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mobileEntity == null)
            {
                logger.LogWarning($"Mobile with ID: {id} not found.");
                return null;
            }
            
            logger.LogInformation($"Found Mobile with ID: {id}.");
            return mapper.Map<Mobile>(mobileEntity);
        }

        public async Task<IEnumerable<Mobile>> GetAllAsync(IFilterDto? filter = null)
        {
            var query = context.Mobiles
                .Include(m => m.StockItem)
                .AsNoTracking();
            
            if (filter is MobileFilterDto mobileFilter)
            {
                query = query.Where(mobileFilter.ToEntityExpression());
                logger.LogInformation($"Filtered Mobiles. Total count: {query.Count()}");
            }
            
            var result =  await query.ToListAsync();
            
            logger.LogInformation($"Receiving all Mobiles. Total count: {result.Count}");
            return result.Select(mapper.Map<Mobile>);
        }

        public async Task<Mobile?> UpdateAsync(int id, Mobile updatedMobile)
        {
            var mobileEntity = await context.Mobiles
                .Include(m => m.StockItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mobileEntity == null)
            {
                logger.LogWarning($"Attempted to update non-existent Mobile with ID: {id}");
                return null;
            }
            
            mapper.Map(updatedMobile, mobileEntity);
            mobileEntity.StockItem.ModifiedAt = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Updated Mobile with ID: {mobileEntity.Id}.");
            return mapper.Map<Mobile>(mobileEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var mobileEntity = await context.Mobiles
                .Include(m => m.StockItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mobileEntity != null)
            {
                context.Mobiles.Remove(mobileEntity);
                context.StockItems.Remove(mobileEntity.StockItem);
                
                await context.SaveChangesAsync();
                logger.LogInformation($"Deleted Mobile with ID: {id}.");
            }
            else logger.LogWarning($"Attempted to delete non-existent Mobile with ID: {id}");
        }
    }
}