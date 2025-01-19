using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPI.Repositories
{
    public interface ITVStorage
    {
        void AddTV(TV tv);
        TV? GetTVById(int id);
        IEnumerable<TV> GetAllTVs();
        TV? UpdateTV(int id, TV updatedTV);
        void DeleteTV(int id);
    }
}