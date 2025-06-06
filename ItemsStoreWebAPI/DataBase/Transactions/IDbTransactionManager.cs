using Microsoft.Data.SqlClient;

namespace ItemsStoreWebAPI.DataBase.Transactions
{
    public interface IDbTransactionManager
    {
        Task ExecuteTransactionAsync(Func<SqlConnection, SqlTransaction, Task> action);
    }
}