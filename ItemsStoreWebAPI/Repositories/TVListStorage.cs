using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Extensions;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Repositories
{
    public class TVListStorage : ITVStorage
    {
        private readonly List<TV> _tvCollection;
        private int _nextId;
        private readonly ILogger<TVListStorage> _logger;

        public TVListStorage(ILogger<TVListStorage> logger)
        {
            _tvCollection = new List<TV>();
            _nextId = 1;
            _logger = logger;
        }

        public TV? AddTV(TV tv)
        {
            if (tv.ID == 0)
                tv.ID = _nextId++;
            
            tv.AddedAt = DateTime.UtcNow;
            //tv.ModifiedAt = DateTime.UtcNow;

            _tvCollection.Add(tv);
            
            _logger.LogInformation($"Added TV with ID: {tv.ID}. {tv}");
            return _tvCollection.First(x => x.ID == tv.ID);
        }

        public TV? GetTVById(int id)
        {
            var tv = _tvCollection.FirstOrDefault(x => x.ID == id);
            
            if (tv != null)
                _logger.LogInformation($"Found TV with ID: {tv.ID}. {tv}");
            else
                _logger.LogWarning($"TV with ID: {id} not found");
            
            return tv;
        }

        public IEnumerable<TV> GetAllTVs(TvFilterDto? filter = null)
        {
            var expression = filter.ToExpression();
            var query = _tvCollection.AsQueryable();

            if (filter != null)
            {
                query = query.Where(expression);
                _logger.LogInformation($"Filtered TVs. Total count: {query.Count()}");
            }
            
            _logger.LogInformation($"Receiving all TVs. Total count: {_tvCollection.Count}");
            return query.ToList();
        }

        public TV? UpdateTV(int id, TV updatedTV)
        {
            var tv = GetTVById(id);

            if (tv != null)
            {
                tv.ID = updatedTV.ID;
                tv.Name = updatedTV.Name;
                tv.Description = updatedTV.Description;
                tv.Size = updatedTV.Size;
                tv.Resolution = updatedTV.Resolution;
                tv.Frequency = updatedTV.Frequency;
                tv.ReleasedYear = updatedTV.ReleasedYear;
                tv.Price = updatedTV.Price;
                tv.ModifiedAt = DateTime.UtcNow;
                tv.InStock = updatedTV.InStock;
                
                _logger.LogInformation($"TV with ID: {id}, updated successfully. {tv}");
            }
            else
                _logger.LogWarning($"Attempted to update non-existent TV with ID: {id}");

            return tv;
        }

        public void DeleteTV(int id)
        {
            var tv = GetTVById(id);

            if (tv != null)
            {
                _tvCollection.Remove(tv);
                _logger.LogInformation($"TV with ID: {id}, deleted successfully");
            }
            else
                _logger.LogWarning($"Attempted to delete non-existent TV with ID: {id}");
        }
    }
}