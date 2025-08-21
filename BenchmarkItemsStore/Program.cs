using System.Diagnostics;
using BenchmarkItemsStore.Configurations;
using BenchmarkItemsStore.Services.Implementations;
using BenchmarkItemsStore.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BenchmarkItemsStore
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var options = new PerformanceOptions();
            config.GetSection("PerformanceTest").Bind(options);

            var category = options.Category.Trim().ToLowerInvariant();

            IStorePerformance storePerformance = category switch
            {
                "mobile" => new MobileStorePerformance(options.BaseUrls.Mobile),
                "tv" => new TvStorePerformance(options.BaseUrls.TV),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var totalPerformanceTimer = Stopwatch.StartNew();
            
            await storePerformance.RunPreloadTestAsync();
            await storePerformance.RunAsync(
                count: options.Count,
                minPrice: options.MinPrice,
                maxPrice: options.MaxPrice);
            
            totalPerformanceTimer.Stop();
            Console.WriteLine($"Entire running time: {totalPerformanceTimer.ElapsedMilliseconds} ms.");
        }
    }
}