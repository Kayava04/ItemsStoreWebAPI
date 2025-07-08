using ItemsStoreWebAPI.DataBase.Transactions;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public class MobileDbTransactionsService(IDbTransactionOperations<Mobile> mobileDbTransactions) : IDbTransactionsService<Mobile>
    {
        public async Task<IEnumerable<Mobile>> AddMultipleAsync(IEnumerable<Mobile> mobiles)
        {
            return await mobileDbTransactions.AddMultipleAsync(mobiles);
        }

        public async Task<IEnumerable<Mobile>> UpdateMultipleAsync(IEnumerable<Mobile> mobiles)
        {
            return await mobileDbTransactions.UpdateMultipleAsync(mobiles);
        }

        public async Task DeleteMultipleAsync(IEnumerable<Mobile> mobiles)
        {
            await mobileDbTransactions.DeleteMultipleAsync(mobiles);
        }
    }
}