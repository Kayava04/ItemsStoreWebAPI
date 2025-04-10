using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Repositories
{
    public interface ITVStorage
    {
        TV? AddTV(TV tv);
        TV? GetTVById(int id);
        IEnumerable<TV> GetAllTVs(TvFilterDto? filter = null);
        TV? UpdateTV(int id, TV updatedTV);
        void DeleteTV(int id);
    }
}