using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Repositories
{
    public interface ITvBatchRepository
    {
        Task<IEnumerable<TV>> AddMultipleTVsAsync(IEnumerable<TV> tvs);
    }
}