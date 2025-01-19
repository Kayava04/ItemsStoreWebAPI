using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPI.Repositories
{
    public class TVStorage : ITVStorage
    {
        private readonly List<TV> _tvStorage;
        private int _countOfElements;

        public TVStorage()
        {
            _tvStorage = new List<TV>();
        }

        public void AddTV(TV tv)
        {
            tv.ID = ++_countOfElements;
            tv.AddedAt = DateTime.UtcNow;
            //tv.ModifiedAt = DateTime.UtcNow;

            _tvStorage.Add(tv);
        }

        public TV? GetTVById(int id)
        {
            return _tvStorage.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<TV> GetAllTVs()
        {
            return _tvStorage;
        }

        public TV? UpdateTV(int id, TV updatedTV)
        {
            var tv = GetTVById(id);

            if (tv != null)
            {
                tv.Name = updatedTV.Name;
                tv.Description = updatedTV.Description;
                tv.Size = updatedTV.Size;
                tv.Resolution = updatedTV.Resolution;
                tv.Frequency = updatedTV.Frequency;
                tv.ReleasedYear = updatedTV.ReleasedYear;
                tv.Price = updatedTV.Price;
                tv.ModifiedAt = DateTime.UtcNow;
                tv.InStock = updatedTV.InStock;
            }

            return tv;
        }

        public void DeleteTV(int id)
        {
            var tv = GetTVById(id);

            if (tv != null)
                _tvStorage.Remove(tv);
        }
    }
}