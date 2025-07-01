using ItemsStoreWebAPI.DTOs;

namespace ItemsStoreWebAPI.Repositories
{
    public interface IStorageBase<T> where T : class
    {
        Task<T?> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(IFilterDto? filter = null);
        Task<T?> UpdateAsync(int id, T updatedEntity);
        Task DeleteAsync(int id);
    }
}