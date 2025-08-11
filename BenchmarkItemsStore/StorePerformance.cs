using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using ItemsStoreWebAPI.Application.DTOs.TV;
using log4net;
using log4net.Config;

namespace BenchmarkItemsStore
{
    public class StorePerformance
    {
        private static HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
            {
                MaxConnectionsPerServer = 50
            })
        {
            BaseAddress = new Uri("http://localhost:5117/v1/stock/electronic/tv/")
        };

        private static int _lastId;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public StorePerformance()
        {
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config.xml"));
        }
        
        // Get Last Index of Elements
        private async Task GetLastIdAsync()
        {
            var response = await _httpClient.GetAsync("filter");
            var tvs = await response.Content.ReadFromJsonAsync<List<RequestTvDto>>();
            
            _lastId = tvs?.Any() == true ? tvs.Max(tv => tv.Id) : 0;
        }
        
        // Generating Range
        private static IEnumerable<int> GenerateRange(int lastId, int count)
        {
            var start = Math.Max(1, lastId - count + 1);
            return Enumerable.Range(start, count);
            
            // Enumerable.Range(lastId - count + 1, count);
        }
        
        // Preload test
        public async Task RunPreloadTestAsync()
        {
            _logger.Info("Starting preload tests...");
            await GetAllItemsAsync();
            _logger.Info("Preload tests finished.");
        }
        
        // Performance test
        public async Task RunAsync(int count, decimal minPrice, decimal maxPrice)
        {
            _logger.Info("Loading store performance tests...");
            
            // Add items timer
            var addItemsTimer = Stopwatch.StartNew();
            await AddItemsAsync(count);
            addItemsTimer.Stop();
            _logger.Info($"{nameof(AddItemsAsync)} finished. Total time: {addItemsTimer.ElapsedMilliseconds} ms.");
            
            await GetLastIdAsync();
            
            // Get all items timer
            var getAllItemsTimer = Stopwatch.StartNew();
            await GetAllItemsAsync();
            getAllItemsTimer.Stop();
            _logger.Info($"{nameof(GetAllItemsAsync)} finished. Total time: {getAllItemsTimer.ElapsedMilliseconds} ms.");
            
            // Update items timer
            var updateItemsTimer = Stopwatch.StartNew();
            await UpdateItemsAsync(count);
            updateItemsTimer.Stop();
            _logger.Info($"{nameof(UpdateItemsAsync)} finished. Total time: {updateItemsTimer.ElapsedMilliseconds} ms.");
            
            // Get items by ID timer
            var getItemsByIdTimer = Stopwatch.StartNew();
            await GetItemsByIdAsync(count);
            getItemsByIdTimer.Stop();
            _logger.Info($"{nameof(GetItemsByIdAsync)} finished. Total time: {getItemsByIdTimer.ElapsedMilliseconds} ms.");
            
            // Delete items timer
            var deleteItemsTimer = Stopwatch.StartNew();
            await DeleteItemsAsync(count);
            deleteItemsTimer.Stop();
            _logger.Info($"{nameof(DeleteItemsAsync)} finished. Total time: {deleteItemsTimer.ElapsedMilliseconds} ms.");
            
            await GetAllItemsAsync();
            
            await GetLastIdAsync();
            
            // Add multiple items timer
            var addMultipleTimer = Stopwatch.StartNew();
            await AddMultipleItemsAsync(count);
            addMultipleTimer.Stop();
            _logger.Info($"{nameof(AddMultipleItemsAsync)} finished. Total time: {addMultipleTimer.ElapsedMilliseconds} ms.");
            
            await GetLastIdAsync();
            
            // Update multiple items timer
            var updateMultipleTimer = Stopwatch.StartNew();
            await UpdateMultipleItemsAsync(count);
            updateMultipleTimer.Stop();
            _logger.Info($"{nameof(UpdateMultipleItemsAsync)} finished. Total time: {updateMultipleTimer.ElapsedMilliseconds} ms.");
            
            // Delete multiple items timer
            var deleteMultipleTimer = Stopwatch.StartNew();
            await DeleteMultipleItemsAsync(count);
            deleteMultipleTimer.Stop();
            _logger.Info($"{nameof(DeleteMultipleItemsAsync)} finished. Total time: {deleteMultipleTimer.ElapsedMilliseconds} ms.");
            
            await GetAllItemsAsync();
            
            _logger.Info("Store performance tests completed.");
        }
        
        #region Single Methods
        
        private static async Task AddItemsAsync(int count)
        {
            var tasks = Enumerable.Range(1, count).Select(async i =>
            {
                var tv = new RequestTvDto
                {
                    Name = $"LG {i}",
                    Description = $"OLED {i}",
                    ScreenSize = 55,
                    Resolution = "2560x1440",
                    Frequency = 120,
                    ReleasedYear = 2024,
                    Price = 23700,
                    InStock = 7
                };
                
                var response = await _httpClient.PostAsJsonAsync(string.Empty, tv);

                if (response.IsSuccessStatusCode)
                {
                    var created = await response.Content.ReadFromJsonAsync<ResponseTvDto>();
                    _logger.Info($"Successfully added TV with ID: {created?.Id}.");
                }
                else
                    _logger.Error($"Failed to add TV. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }

        private static async Task GetItemsByIdAsync(int count)
        {
            var tasks = GenerateRange(_lastId, count).Select(async i =>
            {
                var response = await _httpClient.GetAsync(i.ToString());
                var tv = await response.Content.ReadFromJsonAsync<RequestTvDto>();
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully received TV with ID: {tv.Id}");
                else
                    _logger.Error($"Failed to get TV. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
        
        private static async Task GetAllItemsAsync()
        {
            var response = await _httpClient.GetAsync("filter");
            var tvs = await response.Content.ReadFromJsonAsync<List<RequestTvDto>>();
            
            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully received all TVs. Total count: {tvs.Count}");
            else
                _logger.Error($"Failed to get all TVs. Status Code: {response.StatusCode}");
        }
        
        private static async Task UpdateItemsAsync(int count)
        {
            var tasks = GenerateRange(_lastId, count).Select(async i =>
            {
                var updatedTV = new RequestTvDto
                {
                    Id = i,
                    Name = $"LG {i} updated",
                    Description = $"OLED {i} updated",
                    ScreenSize = 55,
                    Resolution = "2560x1440",
                    Frequency = 120,
                    ReleasedYear = 2024,
                    Price = 23700,
                    InStock = 7
                };
                
                var response = await _httpClient.PutAsJsonAsync(string.Empty, updatedTV);
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully updated TV with ID: {updatedTV.Id}");
                else
                    _logger.Error($"Failed to update TV. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
        
        private static async Task DeleteItemsAsync(int count)
        {
            var tasks = GenerateRange(_lastId, count).Select(async i =>
            {
                var response = await _httpClient.DeleteAsync(i.ToString());
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully deleted TV with ID: {i}");
                else
                    _logger.Error($"Failed to delete TV. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
        
        #endregion

        #region Multiple Methods
        
        private static async Task AddMultipleItemsAsync(int count)
        {
            var tvs = Enumerable.Range(1, count).Select(i => new RequestTvDto
            {
                Name = $"LG Multiple {i}",
                Description = $"OLED Multiple {i}",
                ScreenSize = 55,
                Resolution = "3840x2160",
                Frequency = 120,
                ReleasedYear = 2024,
                Price = 25000,
                InStock = 5
            }).ToList();

            var response = await _httpClient.PostAsJsonAsync("add-multiple", tvs);

            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully added multiple TVs. Count: {count}");
            else
                _logger.Error($"Failed to add multiple TVs. Status Code: {response.StatusCode}");
        }
        
        private static async Task UpdateMultipleItemsAsync(int count)
        {
            var ids = GenerateRange(_lastId, count).ToList();

            var updatedTVs = ids.Select(i => new RequestTvDto
            {
                Id = i,
                Name = $"LG Multiple Updated {i}",
                Description = $"OLED Multiple Updated {i}",
                ScreenSize = 65,
                Resolution = "7680x4320",
                Frequency = 144,
                ReleasedYear = 2025,
                Price = 33000,
                InStock = 3
            }).ToList();

            var response = await _httpClient.PutAsJsonAsync("update-multiple", updatedTVs);

            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully updated multiple TVs. Count: {count}");
            else
                _logger.Error($"Failed to update multiple TVs. Status Code: {response.StatusCode}");
        }

        private static async Task DeleteMultipleItemsAsync(int count)
        {
            var ids = GenerateRange(_lastId, count).ToList();

            var request = new HttpRequestMessage(HttpMethod.Delete, "delete-multiple")
            {
                Content = JsonContent.Create(ids)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully deleted multiple TVs. Count: {count}");
            else
                _logger.Error($"Failed to delete multiple TVs. Status Code: {response.StatusCode}");
        }

        #endregion
    }
}