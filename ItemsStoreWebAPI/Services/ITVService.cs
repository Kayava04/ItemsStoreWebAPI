using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public interface ITVService
    {
        Task<TV?> AddTV(TV tv);
        Task<TV?> GetTVById(int id);
        Task<IEnumerable<TV>> GetTVs(TvFilterDto? filter = null);
        Task<TV?> UpdateTV(int id, TV updatedTV);
        Task DeleteTV(int id);
    }
}