using AutoMapper;
using ItemsStoreWebAPI.Core;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Repositories
{
    public class TvBatchRepository(
        BaseDbContext context,
        IMapper mapper,
        ILogger<TvBatchRepository> logger) : ITvBatchRepository
    {
        public async Task<IEnumerable<TV>> AddMultipleTVsAsync(IEnumerable<TV> tvs)
        {
            var entities = tvs.Select(tv =>
            {
                var entity = mapper.Map<TvEntity>(tv);
                return entity;
            }).ToList();
            
            await context.AddRangeAsync(entities);
            logger.LogInformation($"Insert of {entities.Count} TVs completed.");
            
            return entities.Select(mapper.Map<TV>).ToList();
        }
    }
}