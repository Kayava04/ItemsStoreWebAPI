namespace BenchmarkItemsStore.Services.Interfaces
{
    public interface IStorePerformance
    {
        Task RunPreloadTestAsync();
        Task RunAsync(int count, decimal minPrice, decimal maxPrice);
    }
}