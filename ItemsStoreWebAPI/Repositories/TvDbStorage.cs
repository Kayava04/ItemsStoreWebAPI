using ItemsStoreWebAPI.Core;
using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Extensions;
using ItemsStoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Repositories
{
    public class TvDbStorage(ItemsStoreDbContext context, ILogger<TvDbStorage> logger)
        : ITVStorage
    {
        //TODO: Change 'int' ID in models on 'Guid'
        //      Make all interface methods async
        //      Modify realization methods in this storage
        
        public TV? AddTV(TV tv)
        {
            var stockItem = new StockItemEntity
            {
                Id = Guid.NewGuid(),
                Price = tv.Price,
                InStock = tv.InStock,
                AddedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            var tvEntity = new TvEntity
            {
                Id = Guid.NewGuid(),
                StockItemId = stockItem.Id,
                Name = tv.Name,
                Description = tv.Description,
                Size = tv.Size,
                Resolution = tv.Resolution,
                Frequency = tv.Frequency,
                ReleasedYear = tv.ReleasedYear,
                StockItem = stockItem
            };
            
            context.TVs.Add(tvEntity);
            context.SaveChanges();
            
            logger.LogInformation($"Added TV with ID: {tv.ID}.");
            return MapToTv(tvEntity);
        }

        public TV? GetTVById(int id)
        {
            var tvEntity = context.TVs
                .AsNoTracking()
                .FirstOrDefault(tv => tv.Id == id);

            logger.LogInformation($"Found TV with ID: {id}.");
            return MapToTv(tvEntity);
        }

        public IEnumerable<TV> GetTVs(TvFilterDto? filter = null)
        {
            var query = context.TVs
                .Include(tv => tv.StockItem)
                .AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter.ToEntityExpression());
                logger.LogInformation($"Filtered TVs. Total count: {query.Count()}");
            }
            
            var result =  query.ToList();
            
            logger.LogInformation($"Receiving all TVs. Total count: {result.Count}");
            return result.Select(MapToTv);
        }

        public TV? UpdateTV(int id, TV updatedTV)
        {
            var tvEntity = context.TVs
                .FirstOrDefault(tv => tv.Id == id);

            if (tvEntity == null)
            {
                logger.LogWarning($"Attempted to update non-existent TV with ID: {id}");
                return null;
            }
            
            // StockItemEntity
            tvEntity.StockItem.Price = updatedTV.Price;
            tvEntity.StockItem.InStock = updatedTV.InStock;
            tvEntity.StockItem.ModifiedAt = DateTime.UtcNow;
            
            // TvEntity
            tvEntity.Name = updatedTV.Name;
            tvEntity.Description = updatedTV.Description;
            tvEntity.Size = updatedTV.Size;
            tvEntity.Resolution = updatedTV.Resolution;
            tvEntity.Frequency = updatedTV.Frequency;
            tvEntity.ReleasedYear = updatedTV.ReleasedYear;

            context.SaveChanges();
            
            logger.LogInformation($"Updated TV with ID: {tvEntity.Id}.");
            return MapToTv(tvEntity);
        }

        public void DeleteTV(int id)
        {
            var tvEntity = context.TVs
                .Include(tv => tv.StockItem)
                .FirstOrDefault(tv => tv.Id == id);

            if (tvEntity != null)
            {
                context.TVs.Remove(tvEntity);
                context.SaveChanges();

            }
            else logger.LogWarning($"Attempted to delete non-existent TV with ID: {id}");
        }

        // Test Mapper
        //TODO: Switch on AutoMapper
        private TV MapToTv(TvEntity? tvEntity)
        {
            return new TV
            {
                // ID = tvEntity.Id,
                Name = tvEntity.Name,
                Description = tvEntity.Description,
                Size = tvEntity.Size,
                Resolution = tvEntity.Resolution,
                Frequency = tvEntity.Frequency,
                ReleasedYear = tvEntity.ReleasedYear,
                Price = tvEntity.StockItem.Price,
                InStock = tvEntity.StockItem.InStock,
                AddedAt = tvEntity.StockItem.AddedAt,
                ModifiedAt = tvEntity.StockItem.ModifiedAt
            };
        }
    }
}