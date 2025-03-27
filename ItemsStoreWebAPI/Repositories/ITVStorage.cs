using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPI.Repositories
{
    public interface ITVStorage
    {
        TV? AddTV(TV tv);
        TV? GetTVById(int id);
        IEnumerable<TV> GetAllTVs();
        IEnumerable<TV> GetFilteredTVs();
        TV? UpdateTV(int id, TV updatedTV);
        void DeleteTV(int id);
    }
}