using System.Diagnostics;


namespace BenchmarkItemsStore
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var storePerformance = new StorePerformance();
            
            var totalPerformanceTimer = Stopwatch.StartNew();
            await storePerformance.RunPreloadTestAsync();
            await storePerformance.RunAsync(count: 100000, minPrice: 10599, maxPrice: 52000);
            totalPerformanceTimer.Stop();

            Console.WriteLine($"Entire running time: {totalPerformanceTimer.ElapsedMilliseconds} ms.");
        }
    }
}