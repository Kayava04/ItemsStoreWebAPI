namespace BenchmarkItemsStore
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var storePerformance = new StorePerformance();
            await storePerformance.RunPreloadTestAsync(5);
            await storePerformance.RunAsync(count: 1000);
        }
    }
}