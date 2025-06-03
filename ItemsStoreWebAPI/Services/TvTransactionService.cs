using ItemsStoreWebAPI.Models;
using ItemsStoreWebAPI.Repositories;

namespace ItemsStoreWebAPI.Services
{
    public class TvTransactionService(
        ITvBatchRepository batchRepository,
        ISqlTransactionRepository transactionRepository) : ITvTransactionService
    {
        public async Task<IEnumerable<TV>> AddMultipleTVsAsync(IEnumerable<TV> tvs)
        {
            IEnumerable<TV> addedTvs = [];

            await transactionRepository.ExecuteTransactionAsync(async () =>
            {
                addedTvs = await batchRepository.AddMultipleTVsAsync(tvs);            
            });
            
            return addedTvs;
        }
    }
}