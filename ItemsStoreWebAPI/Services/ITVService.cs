using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public interface ITVService
    {
        TV AddTV(TV tv);
        TV? GetTVById(int id);
        IEnumerable<TV> GetTVs(TvFilterDto? filter = null);
        TV? UpdateTV(int id, TV updatedTV);
        void DeleteTV(int id);
    }
}