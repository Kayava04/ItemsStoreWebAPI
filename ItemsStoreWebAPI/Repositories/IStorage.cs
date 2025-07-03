using ItemsStoreWebAPI.DTOs;

namespace ItemsStoreWebAPI.Repositories
{
    public interface IStorage<T> where T : class
    {
        Task<T?> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(IFilterDto? filter = null);
        Task<T?> UpdateAsync(int id, T updatedEntity);
        Task DeleteAsync(int id);
    }
    
    /*
     * TODO:
     * 1. Realize TV/MobileFileService like generic type (+)
     * 2. Get connection string with IOptions in DI (+)
     * 3. Rename repo and service interfaces (+)
     * 4. Implement Mobile repo with using Dapper (+)
     * 5. Implement db transactions for mobile with using Dapper (+)
     * 6. Change registration of FluentValidation in DI (+)
     * 7. Create Migrations (+)
     * 8. Implement methods for testing api transactions and for mobile in StorePerformance (-)
     * 9. Check best practise of using db connection string (-)
     * 10. Read about using await in the open connection (-)
     * 11. Validate behavior unexpected inputs (-)
     */
}