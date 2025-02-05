using Microsoft.Extensions.Logging;


namespace BenchmarkItemsStore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            
            var logger = loggerFactory.CreateLogger<StorePerformance>();
            
            var loadTestItems = new StorePerformance(logger);
            await loadTestItems.RunAsync();
        }
    }
}