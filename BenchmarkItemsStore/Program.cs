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
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                var options = new PerformanceOptions();
                config.GetSection("PerformanceTest").Bind(options);

                ValidateOptions(options);

                var category = options.Category.Trim().ToLowerInvariant();

                var baseUrl = BuildBaseUrl(options.StockApi, category);

                IStorePerformance storePerformance = category switch
                {
                    "mobile" => new MobileStorePerformance(baseUrl),
                    "tv" => new TvStorePerformance(baseUrl),
                    _ => throw new ArgumentOutOfRangeException(nameof(options.Category),
                        $"Unsupported category '{options.Category}'.")
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
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private static void ValidateOptions(PerformanceOptions o)
        {
            if (o == null) throw new ArgumentNullException(nameof(o));
            if (string.IsNullOrWhiteSpace(o.Category))
                throw new ArgumentException("PerformanceTest:Category must be provided!");
            if (o.StockApi == null)
                throw new ArgumentException("PerformanceTest:StockApi must be provided!");
            if (string.IsNullOrWhiteSpace(o.StockApi.Protocol))
                throw new ArgumentException("PerformanceTest:StockApi:Protocol must be provided!");
            if (string.IsNullOrWhiteSpace(o.StockApi.Host))
                throw new ArgumentException("PerformanceTest:StockApi:Host must be provided!");
            if (o.StockApi.Port <= 0 || o.StockApi.Port > 65535)
                throw new ArgumentException("PerformanceTest:StockApi:Port must be a positive number!");
            if (string.IsNullOrWhiteSpace(o.StockApi.BasePath))
                throw new ArgumentException("PerformanceTest:StockApi:BasePath must be provided!");
        }

        private static string BuildBaseUrl(StockApiOptions api, string category)
        {
            var cleanedBasePath = api.BasePath.Trim('/');
            var cleanedCategory = category.Trim('/').ToLowerInvariant();

            var uri = new UriBuilder
            {
                Scheme = api.Protocol.Trim(),
                Host = api.Host.Trim(),
                Port = api.Port,
                Path = $"{cleanedBasePath}/{cleanedCategory}/"
            }.Uri;

            return uri.ToString();
        }
    }
}