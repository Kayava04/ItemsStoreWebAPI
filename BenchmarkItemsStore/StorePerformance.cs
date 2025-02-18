using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using ItemsStoreWebAPI.Models;
using log4net;
using log4net.Config;


namespace BenchmarkItemsStore
{
    public class StorePerformance
    {
        private static HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5117/v1/stock/electronic/tv/")
        };

        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public StorePerformance()
        {
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config.xml"));
        }

        public async Task RunAsync(string method = null, int tvCount = 0, int id = 0)
        {
            _logger.Info("Loading store performance tests...");
            var timer = Stopwatch.StartNew();
            
            switch (method)
            {
                case nameof(AddItemsAsync):
                    await AddItemsAsync(tvCount);
                    break;
                case nameof(GetItemsByIdAsync):
                    await GetItemsByIdAsync(id);
                    break;
                case nameof(GetAllItemsAsync):
                    await GetAllItemsAsync();
                    break;
                case nameof(UpdateItemsAsync):
                    await UpdateItemsAsync();
                    break;
                case nameof(DeleteItemsAsync):
                    await DeleteItemsAsync(id);
                    break;
                default:
                    _logger.Warn($"Method {method} is not supported.");
                    break;
            }

            timer.Stop();
            _logger.Info($"{method} completed. Total time: {timer.ElapsedMilliseconds} ms.");
        }
        
        public async Task AddItemsAsync(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                var tv = new TV
                {
                    Name = $"LG {i}",
                    Description = $"OLED {i}",
                    Size = 55,
                    Resolution = "2560x1440",
                    Frequency = 120,
                    ReleasedYear = 2024,
                    Price = 23700,
                    InStock = 7
                };

                var response = await _httpClient.PostAsJsonAsync(String.Empty, tv);

                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully added TV with ID: {i}");
                else
                    _logger.Error($"Failed to add TV with ID {i}. Status Code: {response.StatusCode}");
            }
        }

        public async Task GetItemsByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(id.ToString());
            var tv = await response.Content.ReadFromJsonAsync<TV>();
            
            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully received TV with ID: {tv.ID}");
            else
                _logger.Error($"Failed to get TV. Status Code: {response.StatusCode}");
        }
        
        public async Task GetAllItemsAsync()
        {
            var response = await _httpClient.GetAsync(String.Empty);
            var tvs = await response.Content.ReadFromJsonAsync<List<TV>>();
            
            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully received all TVs. Total count: {tvs.Count}");
            else
                _logger.Error($"Failed to get all TVs. Status Code: {response.StatusCode}");
        }

        public async Task UpdateItemsAsync()
        {
            var updatedTV = new TV
            {
                ID = 1,
                Name = "LG",
                Description = "OLED",
                Size = 55,
                Resolution = "2560x1440",
                Frequency = 120,
                ReleasedYear = 2024,
                Price = 23700,
                InStock = 7
            };
            
            var response = await _httpClient.PutAsJsonAsync(String.Empty, updatedTV);
            
            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully updated TV with ID: {updatedTV.ID}");
            else
                _logger.Error($"Failed to update TV. Status Code: {response.StatusCode}");
        }

        public async Task DeleteItemsAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(id.ToString());
            
            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully deleted TV with ID: {id}");
            else
                _logger.Error($"Failed to delete TV. Status Code: {response.StatusCode}");
        }
    }
}