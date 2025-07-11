using AutoMapper;
using ItemsStoreWebAPI.Application.DTOs.Interfaces;
using ItemsStoreWebAPI.Application.DTOs.TV;
using ItemsStoreWebAPI.Application.Extensions;
using ItemsStoreWebAPI.Application.Models;
using ItemsStoreWebAPI.DataAccess.DataBase.DbContexts;
using ItemsStoreWebAPI.DataAccess.Repositories.Interfaces;
using ItemsStoreWebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.DataAccess.Repositories.Implementations
{
    public class TvDbStorage(
        BaseDbContext context,
        IMapper mapper,
        ILogger<TvDbStorage> logger) : IStorage<TV>
    {
        public async Task<TV?> AddAsync(TV tv)
        {
            var tvEntity = mapper.Map<TvEntity>(tv);
            
            await context.TVs.AddAsync(tvEntity);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Added TV with ID: {tv.Id}");
            return mapper.Map<TV>(tvEntity);
        }

        public async Task<TV?> GetByIdAsync(int id)
        {
            var tvEntity = await context.TVs
                .AsNoTracking()
                .Include(t => t.StockItem)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tvEntity == null)
            {
                logger.LogWarning($"TV with ID: {id} not found");
                return null;
            }
            
            logger.LogInformation($"Found TV with ID: {id}");
            return mapper.Map<TV>(tvEntity);
        }

        public async Task<IEnumerable<TV>> GetAllAsync(IFilterDto? filter = null)
        {
            var query = context.TVs
                .Include(tv => tv.StockItem)
                .AsNoTracking();
            
            if (filter is TvFilterDto tvFilter)
            {
                query = query.Where(tvFilter.ToEntityExpression());
                logger.LogInformation($"Filtered TVs. Total count: {query.Count()}");
            }
            
            var result =  await query.ToListAsync();
            
            logger.LogInformation($"Receiving all TVs. Total count: {result.Count}");
            return result.Select(mapper.Map<TV>);
        }

        public async Task<TV?> UpdateAsync(int id, TV updatedTv)
        {
            var tvEntity = await context.TVs
                .Include(t => t.StockItem)
                .FirstOrDefaultAsync(tv => tv.Id == id);

            if (tvEntity == null)
            {
                logger.LogWarning($"Attempted to update non-existent TV with ID: {id}");
                return null;
            }
            
            mapper.Map(updatedTv, tvEntity);
            tvEntity.StockItem.ModifiedAt = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Updated TV with ID: {tvEntity.Id}");
            return mapper.Map<TV>(tvEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var tvEntity = await context.TVs
                .Include(tv => tv.StockItem)
                .FirstOrDefaultAsync(tv => tv.Id == id);

            if (tvEntity != null)
            {
                context.TVs.Remove(tvEntity);
                context.StockItems.Remove(tvEntity.StockItem);
                
                await context.SaveChangesAsync();
                logger.LogInformation($"Deleted TV with ID: {id}");
            }
            else logger.LogWarning($"Attempted to delete non-existent TV with ID: {id}");
        }
    }
}