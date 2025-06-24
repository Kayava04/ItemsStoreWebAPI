namespace ItemsStoreWebAPI.Services
{
    public interface IDbTransactionsService<T> where T : class
    {
        Task<IEnumerable<T>> AddMultipleAsync(IEnumerable<T> items);
        Task<IEnumerable<T>> UpdateMultipleAsync(IEnumerable<T> items);
        Task DeleteMultipleAsync(IEnumerable<T> items);
    }
}