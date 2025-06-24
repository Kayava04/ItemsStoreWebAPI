using Microsoft.Data.SqlClient;

namespace ItemsStoreWebAPI.DataBase.Transactions
{
    public abstract class BaseDbTransactions<T>(string connectionString)
        : IDbTransactionOperations<T> where T : class
    {
        private async Task<TResult> ExecuteTransactionAsync<TResult>(Func<SqlConnection, SqlTransaction, Task<TResult>> operation)
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            await using var transaction = connection.BeginTransaction();

            try
            {
                var result = await operation(connection, transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        
        private async Task ExecuteTransactionAsync(
            Func<SqlConnection, SqlTransaction, Task> operation)
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await using var transaction = connection.BeginTransaction();

            try
            {
                await operation(connection, transaction);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        
        public Task<IEnumerable<T>> AddMultipleAsync(IEnumerable<T> items)
        {
            return ExecuteTransactionAsync((conn, tran) => AddMultipleOperation(conn, tran, items));
        }

        public Task<IEnumerable<T>> UpdateMultipleAsync(IEnumerable<T> items)
        {
            return ExecuteTransactionAsync((conn, tran) => UpdateMultipleOperation(conn, tran, items));
        }

        public Task DeleteMultipleAsync(IEnumerable<T> items)
        {
            return ExecuteTransactionAsync((conn, tran) => DeleteMultipleOperation(conn, tran, items));
        }

        protected abstract Task<IEnumerable<T>> AddMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<T> items);
        protected abstract Task<IEnumerable<T>> UpdateMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<T> items);
        protected abstract Task DeleteMultipleOperation(SqlConnection connection, SqlTransaction transaction, IEnumerable<T> items);
    }
}