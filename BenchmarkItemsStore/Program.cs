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
            await storePerformance.RunAsync(count: 1000);
            totalPerformanceTimer.Stop();

            Console.WriteLine($"Entire running time: {totalPerformanceTimer.ElapsedMilliseconds} ms.");
        }
        
        // Running time: 7001, 7100, 6903, 4836, 5045, 7142, 6977
        //TODO: Check entire benchmark running time with run preload test async 1 and 5
    }
}