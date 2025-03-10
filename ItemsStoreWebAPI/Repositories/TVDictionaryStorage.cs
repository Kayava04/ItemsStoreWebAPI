using System.Collections.Concurrent;
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
        
        public TV? AddTV(TV tv)
        {
            var id = Interlocked.Increment(ref _nextId);
            tv.ID = id;
            tv.AddedAt = DateTime.UtcNow;

            if (_tvCollection.TryAdd(tv.ID, tv))
            {
                _logger.LogInformation($"Added TV with ID: {tv.ID}. {tv}");
                return tv;
            }
            
            _logger.LogWarning($"Failed to add TV with ID: {tv.ID}");
            return null;
        }

        public TV? GetTVById(int id)
        {
            if (_tvCollection.TryGetValue(id, out var tv))
            {
                _logger.LogInformation($"Found TV with ID: {id}. {tv}");
                return tv;
            }
            
            _logger.LogWarning($"TV with ID: {id} not found");
            return null;
        }

        public IEnumerable<TV> GetAllTVs()
        {
            _logger.LogInformation($"Receiving all TVs. Total count: {_tvCollection.Count}");
            return _tvCollection.Values;
        }

        public TV? UpdateTV(int id, TV updatedTV)
        {
            if (_tvCollection.TryGetValue(id, out var tv))
            {
                var newTV = new TV
                {
                    Name = updatedTV.Name,
                    Description = updatedTV.Description,
                    Size = updatedTV.Size,
                    Resolution = updatedTV.Resolution,
                    Frequency = updatedTV.Frequency,
                    ReleasedYear = updatedTV.ReleasedYear,
                    Price = updatedTV.Price,
                    InStock = updatedTV.InStock,
                    ModifiedAt = DateTime.UtcNow
                };
            
                if (_tvCollection.TryUpdate(id, newTV, tv))
                {
                    _logger.LogInformation($"Updated TV with ID: {newTV.ID}. {newTV}");
                    return newTV;
                }
                
                _logger.LogWarning($"Failed to update TV with ID: {id}");
            }
            else
                _logger.LogWarning($"Attempted to update non-existent TV with ID: {id}");

            return tv;
        }

        public void DeleteTV(int id)
        {
            if (_tvCollection.TryRemove(id, out _))
                _logger.LogInformation($"TV with ID: {id}, deleted successfully");
            else
                _logger.LogWarning($"Attempted to delete non-existent TV with ID: {id}");
        }
    }
}