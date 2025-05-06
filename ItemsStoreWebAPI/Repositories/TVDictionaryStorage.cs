using System.Collections.Concurrent;
using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Extensions;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Repositories
{
    public class TVDictionaryStorage : ITVStorage
    {
        private readonly ConcurrentDictionary<int, TV> _tvCollection;
        private int _nextId;
        private readonly ILogger<TVDictionaryStorage> _logger;

        public TVDictionaryStorage(ILogger<TVDictionaryStorage> logger)
        {
            _tvCollection = new ConcurrentDictionary<int, TV>();
            _nextId = 0;
            _logger = logger;
        }
        
        public async Task<TV?> AddTV(TV tv)
        {
            var id = Interlocked.Increment(ref _nextId);
            tv.ID = id;
            tv.AddedAt = DateTime.UtcNow;

            if (_tvCollection.TryAdd(tv.ID, tv))
            {
                _logger.LogInformation($"Added TV with ID: {tv.ID}. {tv}");
                return await Task.FromResult(tv);
            }
            
            _logger.LogWarning($"Failed to add TV with ID: {tv.ID}");
            return await Task.FromResult<TV?>(null);
        }

        public async Task<TV?> GetTVById(int id)
        {
            if (_tvCollection.TryGetValue(id, out var tv))
            {
                _logger.LogInformation($"Found TV with ID: {id}. {tv}");
                return await Task.FromResult(tv);
            }
            
            _logger.LogWarning($"TV with ID: {id} not found");
            return await Task.FromResult<TV?>(null);
        }

        public async Task<IEnumerable<TV>> GetTVs(TvFilterDto? filter = null)
        {
            var query = _tvCollection.Values.AsQueryable();

            if (filter != null)
            {
                var expression = filter.ToExpression();
                query = query.Where(expression);
                _logger.LogInformation($"Filtered TVs. Total count: {query.Count()}");
            }
            
            _logger.LogInformation($"Receiving all TVs. Total count: {_tvCollection.Count}");
            return await Task.FromResult(query.ToList());
        }

        public async Task<TV?> UpdateTV(int id, TV updatedTV)
        {
            if (_tvCollection.TryGetValue(id, out var tv))
            {
                var newTV = new TV
                {
                    ID = updatedTV.ID,
                    Name = updatedTV.Name,
                    Description = updatedTV.Description,
                    Size = updatedTV.Size,
                    Resolution = updatedTV.Resolution,
                    Frequency = updatedTV.Frequency,
                    ReleasedYear = updatedTV.ReleasedYear,
                    Price = updatedTV.Price,
                    ModifiedAt = DateTime.UtcNow,
                    InStock = updatedTV.InStock
                };
            
                var success = _tvCollection.TryUpdate(id, newTV, tv);

                if (success)
                {
                    _logger.LogInformation($"Updated TV with ID: {id}. {newTV}");
                    return await Task.FromResult(newTV);
                }
                
                _logger.LogWarning($"Failed to update TV with ID: {id}");
                return await Task.FromResult<TV?>(null);
            }
            
            _logger.LogWarning($"Attempted to update non-existent TV with ID: {id}");
            return await Task.FromResult<TV?>(null);
        }

        public async Task DeleteTV(int id)
        {
            if (_tvCollection.TryRemove(id, out _))
                _logger.LogInformation($"TV with ID: {id}, deleted successfully");
            else
                _logger.LogWarning($"Attempted to delete non-existent TV with ID: {id}");
            
            await Task.CompletedTask;
        }
    }
}