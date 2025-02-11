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
        
        //TODO: Write new functional
        //      Change console for something else, for good debugging logs
        //      Change configurations parameters without code change
        //      Run any method without code change
        //      Change logging for ability to use search
        //      Add import API method
    }
}