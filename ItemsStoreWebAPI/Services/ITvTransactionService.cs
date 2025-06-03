using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public interface ITvTransactionService
    {
        Task<IEnumerable<TV>> AddMultipleTVsAsync(IEnumerable<TV> tvs);
    }
}