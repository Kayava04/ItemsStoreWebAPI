using System.Linq.Expressions;
using ItemsStoreWebAPI.DTOs.Mobile;
using ItemsStoreWebAPI.Entities;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Extensions
{
    public static class MobileFilterExtensions
    {
        public static Expression<Func<Mobile, bool>> ToModelExpression(this MobileFilterDto filter)
        {
            return mobile =>
                (string.IsNullOrEmpty(filter.Name) || mobile.Name.Contains(filter.Name)) &&
                (!filter.MinScreenSize.HasValue || mobile.ScreenSize >= filter.MinScreenSize.Value) &&
                (!filter.MaxScreenSize.HasValue || mobile.ScreenSize <= filter.MaxScreenSize.Value) &&
                (!filter.MinBatteryCapacity.HasValue || mobile.BatteryCapacity >= filter.MinBatteryCapacity.Value) &&
                (!filter.MaxBatteryCapacity.HasValue || mobile.BatteryCapacity <= filter.MaxBatteryCapacity.Value) &&
                (!filter.MinRAM.HasValue || mobile.RAM >= filter.MinRAM.Value) &&
                (!filter.MaxRAM.HasValue || mobile.RAM <= filter.MaxRAM.Value) &&
                (!filter.MinStorage.HasValue || mobile.Storage >= filter.MinStorage.Value) &&
                (!filter.MaxStorage.HasValue || mobile.Storage <= filter.MaxStorage.Value) &&
                (!filter.MinPrice.HasValue || mobile.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || mobile.Price <= filter.MaxPrice.Value) &&
                (!filter.ReleasedYear.HasValue || mobile.ReleasedYear == filter.ReleasedYear.Value) &&
                (!filter.InStock.HasValue || mobile.InStock == filter.InStock.Value);
        }

        public static Expression<Func<MobileEntity, bool>> ToEntityExpression(this MobileFilterDto filter)
        {
            return mobile =>
                (string.IsNullOrEmpty(filter.Name) || mobile.StockItem.Name.Contains(filter.Name)) &&
                (!filter.MinScreenSize.HasValue || mobile.ScreenSize >= filter.MinScreenSize.Value) &&
                (!filter.MaxScreenSize.HasValue || mobile.ScreenSize <= filter.MaxScreenSize.Value) &&
                (!filter.MinBatteryCapacity.HasValue || mobile.BatteryCapacity >= filter.MinBatteryCapacity.Value) &&
                (!filter.MaxBatteryCapacity.HasValue || mobile.BatteryCapacity <= filter.MaxBatteryCapacity.Value) &&
                (!filter.MinRAM.HasValue || mobile.RAM >= filter.MinRAM.Value) &&
                (!filter.MaxRAM.HasValue || mobile.RAM <= filter.MaxRAM.Value) &&
                (!filter.MinStorage.HasValue || mobile.Storage >= filter.MinStorage.Value) &&
                (!filter.MaxStorage.HasValue || mobile.Storage <= filter.MaxStorage.Value) &&
                (!filter.MinPrice.HasValue || mobile.StockItem.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || mobile.StockItem.Price <= filter.MaxPrice.Value) &&
                (!filter.ReleasedYear.HasValue || mobile.StockItem.ReleasedYear == filter.ReleasedYear.Value) &&
                (!filter.InStock.HasValue || mobile.StockItem.InStock == filter.InStock.Value);
        }
    }
}