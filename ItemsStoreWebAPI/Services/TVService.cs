using ItemsStoreWebAPI.DTOs;
using ItemsStoreWebAPI.Factories;
using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;

namespace ItemsStoreWebAPI.Services
{
    public class TVService : ITVService
    {
        private readonly ITVStorage _tvStorage;

        public TVService(ITVStorageFactory tvStorageFactory)
        {
            _tvStorage = tvStorageFactory.CreateStorage();
        }

        public async Task<TV?> AddTV(TV tv)
        {
            return await _tvStorage.AddTV(tv);
        }

        public async Task<TV?> GetTVById(int id)
        {
            return await _tvStorage.GetTVById(id);
        }

        public async Task<IEnumerable<TV>> GetTVs(TvFilterDto? filter = null)
        {
            return await _tvStorage.GetTVs(filter);
        }

        public async Task<TV?> UpdateTV(int id, TV updatedTV)
        {
            return await _tvStorage.UpdateTV(id, updatedTV);
        }

        public async Task DeleteTV(int id)
        {
            await _tvStorage.DeleteTV(id);
        }
    }
}