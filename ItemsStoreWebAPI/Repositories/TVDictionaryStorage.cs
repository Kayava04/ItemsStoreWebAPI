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
        
        public TV AddTV(TV tv)
        {
            _nextId = Interlocked.Increment(ref _nextId);
            tv.ID = _nextId;
            tv.AddedAt = DateTime.UtcNow;

            if (_tvCollection.TryAdd(tv.ID, tv))
                _logger.LogInformation("Added TV with ID: {tv.ID}. {tv}", tv.ID, tv);
            else
                _logger.LogWarning("Failed to add TV with ID: {tv.ID}", tv.ID);
            
            return _tvCollection.First(x => x.Key == tv.ID).Value;
        }

        public TV? GetTVById(int id)
        {
            var tv = _tvCollection.FirstOrDefault(x => x.Key == id);

            if (tv.Value != null)
                _logger.LogInformation("Found TV with ID: {id}. {tv.Value}", id, tv.Value);
            else
                _logger.LogWarning("TV with ID: {id} not found", id);

            return tv.Value;
        }

        public IEnumerable<TV> GetAllTVs()
        {
            _logger.LogInformation("Receiving all TVs. Total count: {_tvCollection.Count}", _tvCollection.Count);
            return _tvCollection.Values;
        }

        public TV? UpdateTV(int id, TV updatedTV)
        {
            var tv = GetTVById(id);

            if (tv != null)
            {
                tv.Name = updatedTV.Name;
                tv.Description = updatedTV.Description;
                tv.Size = updatedTV.Size;
                tv.Resolution = updatedTV.Resolution;
                tv.Frequency = updatedTV.Frequency;
                tv.ReleasedYear = updatedTV.ReleasedYear;
                tv.Price = updatedTV.Price;
                tv.ModifiedAt = DateTime.UtcNow;
                tv.InStock = updatedTV.InStock;
                
                _logger.LogInformation("TV with ID: {id}, updated successfully. {tv}", id, tv);
            }
            else
                _logger.LogWarning("Attempted to update non-existent TV with ID: {id}", id);

            return tv;
        }

        public void DeleteTV(int id)
        {
            var tv = GetTVById(id);
            
            if (tv != null)
            {
                _tvCollection.TryRemove(id, out _);
                _logger.LogInformation("TV with ID: {id}, deleted successfully", id);
            }
            else
                _logger.LogWarning("Attempted to delete non-existent TV with ID: {id}", id);
        }
    }
}