using ItemsStoreWebAPI.Application.DTOs.Interfaces;

namespace ItemsStoreWebAPI.Application.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<T?> AddAsync(T item);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(IFilterDto? filter = null);
        Task<T?> UpdateAsync(int id, T updatedModel);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> AddMultipleAsync(IEnumerable<T> items);
        Task<IEnumerable<T>> UpdateMultipleAsync(IEnumerable<T> items);
        Task DeleteMultipleAsync(IEnumerable<int> ids);
    }
}