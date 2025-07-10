namespace ItemsStoreWebAPI.DataAccess.DataBase.Transactions.Interfaces
{
    public interface IDbTransactionOperations<T> where T : class
    {
        Task<IEnumerable<T>> AddMultipleAsync(IEnumerable<T> items);
        Task<IEnumerable<T>> UpdateMultipleAsync(IEnumerable<T> items);
        Task DeleteMultipleAsync(IEnumerable<T> items);
    }
}