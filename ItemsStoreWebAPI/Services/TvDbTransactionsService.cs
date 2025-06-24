using ItemsStoreWebAPI.DataBase.Transactions;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public class TvDbTransactionsService(IDbTransactionOperations<TV> tvDbTransactions) : IDbTransactionsService<TV>
    {
        public async Task<IEnumerable<TV>> AddMultipleAsync(IEnumerable<TV> tvs)
        {
            return await tvDbTransactions.AddMultipleAsync(tvs);
        }

        public async Task<IEnumerable<TV>> UpdateMultipleAsync(IEnumerable<TV> tvs)
        {
            return await tvDbTransactions.UpdateMultipleAsync(tvs);
        }

        public async Task DeleteMultipleAsync(IEnumerable<TV> tvs)
        {
            await tvDbTransactions.DeleteMultipleAsync(tvs);
        }
    }
}