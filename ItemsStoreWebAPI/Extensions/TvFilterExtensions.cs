using System.Linq.Expressions;
using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Extensions
{
    public static class TvFilterExtensions
    {
        public static Expression<Func<TV, bool>> ToExpression(this TvFilterDto filter)
        {
            return tv =>
                (string.IsNullOrEmpty(filter.Name) || tv.Name.Contains(filter.Name)) &&
                (!filter.MinSize.HasValue || tv.Size >= filter.MinSize.Value) &&
                (!filter.MaxSize.HasValue || tv.Size <= filter.MaxSize.Value) &&
                (!filter.MinPrice.HasValue || tv.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || tv.Price <= filter.MaxPrice.Value) &&
                (!filter.ReleasedYear.HasValue || tv.ReleasedYear == filter.ReleasedYear.Value) &&
                (!filter.InStock.HasValue || tv.InStock == filter.InStock.Value);
        }
    }
}