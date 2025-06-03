namespace ItemsStoreWebAPI.Repositories
{
    public interface ISqlTransactionRepository
    {
        Task ExecuteTransactionAsync(Func<Task> action);
    }
}