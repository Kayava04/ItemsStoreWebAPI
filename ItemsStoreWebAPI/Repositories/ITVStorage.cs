using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPI.Repositories
{
    public interface ITVStorage
    {
        TV? AddTV(TV tv);
        TV? GetTVById(int id);
        IEnumerable<TV> GetAllTVs();
        IEnumerable<TV> GetTVsByPriceFilter(decimal minPrice, decimal maxPrice);
        TV? UpdateTV(int id, TV updatedTV);
        void DeleteTV(int id);
    }
}