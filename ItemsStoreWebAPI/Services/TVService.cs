using System.Linq.Expressions;
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

        public TV AddTV(TV tv)
        {
            return _tvStorage.AddTV(tv);
        }

        public TV? GetTVById(int id)
        {
            return _tvStorage.GetTVById(id);
        }

        public IEnumerable<TV> GetAllTVs()
        {
            return _tvStorage.GetAllTVs();
        }

        public IEnumerable<TV> GetFilteredTVs(Expression<Func<TV, bool>> filter)
        {
            return _tvStorage.GetFilteredTVs(filter);
        }

        public TV? UpdateTV(int id, TV updatedTV)
        {
            return _tvStorage.UpdateTV(id, updatedTV);
        }

        public void DeleteTV(int id)
        {
            _tvStorage.DeleteTV(id);
        }
    }
}