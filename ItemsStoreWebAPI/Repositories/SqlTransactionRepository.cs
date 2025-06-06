using ItemsStoreWebAPI.DataBase;

namespace ItemsStoreWebAPI.Repositories
{
    public class SqlTransactionRepository(
        BaseDbContext context,
        ILogger<SqlTransactionRepository> logger) : ISqlTransactionRepository
    {
        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                await action();
                
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                logger.LogInformation($"SQL transaction commited successfully");
            }
            catch (OperationCanceledException ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "SQL transaction rolled back");
                
                throw;
            }
        }
    }
}