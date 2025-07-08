using System.Linq.Expressions;
using ItemsStoreWebAPI.DTOs.TV;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Extensions
{
    public static class TvFilterExtensions
    {
        public static Expression<Func<TV, bool>> ToModelExpression(this TvFilterDto filter)
        {
            return tv =>
                (string.IsNullOrEmpty(filter.Name) || tv.Name.Contains(filter.Name)) &&
                (!filter.MinSize.HasValue || tv.ScreenSize >= filter.MinSize.Value) &&
                (!filter.MaxSize.HasValue || tv.ScreenSize <= filter.MaxSize.Value) &&
                (!filter.MinPrice.HasValue || tv.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || tv.Price <= filter.MaxPrice.Value) &&
                (!filter.ReleasedYear.HasValue || tv.ReleasedYear == filter.ReleasedYear.Value) &&
                (!filter.InStock.HasValue || tv.InStock == filter.InStock.Value);
        }
        
        public static Expression<Func<TvEntity, bool>> ToEntityExpression(this TvFilterDto filter)
        {
            return tv =>
                (string.IsNullOrEmpty(filter.Name) || tv.StockItem.Name.Contains(filter.Name)) &&
                (!filter.MinSize.HasValue || tv.ScreenSize >= filter.MinSize.Value) &&
                (!filter.MaxSize.HasValue || tv.ScreenSize <= filter.MaxSize.Value) &&
                (!filter.MinPrice.HasValue || tv.StockItem.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || tv.StockItem.Price <= filter.MaxPrice.Value) &&
                (!filter.ReleasedYear.HasValue || tv.StockItem.ReleasedYear == filter.ReleasedYear.Value) &&
                (!filter.InStock.HasValue || tv.StockItem.InStock == filter.InStock.Value);
        }
    }
}