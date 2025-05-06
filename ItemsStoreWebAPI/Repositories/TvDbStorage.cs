using AutoMapper;
using ItemsStoreWebAPI.Core;
using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Extensions;
using ItemsStoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Repositories
{
    public class TvDbStorage(
        ItemsStoreDbContext context,
        IMapper mapper,
        ILogger<TvDbStorage> logger) : ITVStorage
    {
        public async Task<TV?> AddTV(TV tv)
        {
            var tvEntity = mapper.Map<TvEntity>(tv);
            
            await context.TVs.AddAsync(tvEntity);
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Added TV with ID: {tv.ID}.");
            return mapper.Map<TV>(tvEntity);
        }

        public async Task<TV?> GetTVById(int id)
        {
            var tvEntity = await context.TVs
                .AsNoTracking()
                .Include(t => t.StockItem)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tvEntity == null)
            {
                logger.LogWarning($"TV with ID: {id} not found.");
                return null;
            }
            
            logger.LogInformation($"Found TV with ID: {id}.");
            return mapper.Map<TV>(tvEntity);
        }

        public async Task<IEnumerable<TV>> GetTVs(TvFilterDto? filter = null)
        {
            var query = context.TVs
                .Include(tv => tv.StockItem)
                .AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter.ToEntityExpression());
                logger.LogInformation($"Filtered TVs. Total count: {query.Count()}");
            }
            
            var result =  await query.ToListAsync();
            
            logger.LogInformation($"Receiving all TVs. Total count: {result.Count}");
            return result.Select(mapper.Map<TV>);
        }

        public async Task<TV?> UpdateTV(int id, TV updatedTV)
        {
            var tvEntity = await context.TVs
                .Include(t => t.StockItem)
                .FirstOrDefaultAsync(tv => tv.Id == id);

            if (tvEntity == null)
            {
                logger.LogWarning($"Attempted to update non-existent TV with ID: {id}");
                return null;
            }
            
            mapper.Map(updatedTV, tvEntity);
            tvEntity.StockItem.ModifiedAt = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            
            logger.LogInformation($"Updated TV with ID: {tvEntity.Id}.");
            return mapper.Map<TV>(tvEntity);
        }

        public async Task DeleteTV(int id)
        {
            var tvEntity = await context.TVs
                .Include(tv => tv.StockItem)
                .FirstOrDefaultAsync(tv => tv.Id == id);

            if (tvEntity != null)
            {
                context.TVs.Remove(tvEntity);
                await context.SaveChangesAsync();
                logger.LogInformation($"Deleted TV with ID: {id}.");
            }
            else logger.LogWarning($"Attempted to delete non-existent TV with ID: {id}");
        }
    }
}