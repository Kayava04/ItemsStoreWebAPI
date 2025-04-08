using System.Linq.Expressions;
using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPI.Services
{
    public interface ITVService
    {
        TV AddTV(TV tv);
        TV? GetTVById(int id);
        IEnumerable<TV> GetAllTVs();
        IEnumerable<TV> GetFilteredTVs(Expression<Func<TV, bool>> filter);
        TV? UpdateTV(int id, TV updatedTV);
        void DeleteTV(int id);
    }
}