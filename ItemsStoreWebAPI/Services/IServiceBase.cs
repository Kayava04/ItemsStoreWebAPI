using ItemsStoreWebAPI.DTOs;

namespace ItemsStoreWebAPI.Services
{
    public interface IServiceBase<T> where T : class
    {
        Task<T?> AddAsync(T model);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(IFilterDto? filter = null);
        Task<T?> UpdateAsync(int id, T updatedModel);
        Task DeleteAsync(int id);
    }
}