using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;


namespace ItemsStoreWebAPI.Services
{
    public class TVService : ITVService
    {
        private readonly ITVStorage _tvStorage;

        public TVService(ITVStorage tVStorage)
        {
            _tvStorage = tVStorage;
        }

        public TV AddTV(TV tv)
        {
            tv.AddedAt = DateTime.UtcNow;
            //tv.ModifiedAt = DateTime.UtcNow;

            _tvStorage.AddTV(tv);
            return tv;
        }

        public TV? GetTVById(int id)
        {
            return _tvStorage.GetTVById(id);
        }

        public IEnumerable<TV> GetAllTVs()
        {
            return _tvStorage.GetAllTVs();
        }

        public TV? UpdateTV(int id, TV updatedTV)
        {
            updatedTV.ModifiedAt = DateTime.UtcNow;
            return _tvStorage.UpdateTV(id, updatedTV);
        }

        public void DeleteTV(int id)
        {
            //var tv = GetTVById(id);

            //if (tv != null)
                _tvStorage.DeleteTV(id);
        }
    }
}