using Microsoft.Data.SqlClient;

namespace ItemsStoreWebAPI.DataBase.Transactions
{
    public class DbTransactionManager(
        IConfiguration config,
        ILogger<DbTransactionManager> logger) : IDbTransactionManager
    {
        private readonly string? _connectionString = config.GetConnectionString("PostgresDbContext");
        
        public async Task ExecuteTransactionAsync(Func<SqlConnection, SqlTransaction, Task> action)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = connection.BeginTransaction();

            try
            {
                await action(connection, transaction);
                
                await transaction.CommitAsync();
                logger.LogInformation("Sql transaction completed successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "Sql transaction rolled back");
                
                throw;
            }
        }
    }
}