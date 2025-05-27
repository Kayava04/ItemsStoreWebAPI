using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using ItemsStoreWebAPI.DTOs;
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
        private static readonly List<int> _recentlyAddedIds = [];
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
            
            _lastId = tvs?.Any() == true ? tvs.Max(tv => tv.ID) : 0;
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

            await GetLastIdAsync();
            
            // Add items timer
            var addItemsTimer = Stopwatch.StartNew();
            await AddItemsAsync(count);
            addItemsTimer.Stop();
            _logger.Info($"{nameof(AddItemsAsync)} finished. Total time: {addItemsTimer.ElapsedMilliseconds} ms.");
            
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
            
            _logger.Info("Store performance tests completed.");
        }
        
        private static async Task AddItemsAsync(int count)
        {
            _recentlyAddedIds.Clear();
            
            var tasks = Enumerable.Range(1, count).Select(async i =>
            {
                var tv = new RequestTvDto
                {
                    // ID = ++_lastId,
                    Name = $"LG {_lastId}",
                    Description = $"OLED {_lastId}",
                    Size = 55,
                    Resolution = "2560x1440",
                    Frequency = 120,
                    ReleasedYear = 2024,
                    Price = 23700,
                    InStock = 7
                };
                
                var response = await _httpClient.PostAsJsonAsync(string.Empty, tv);

                if (response.IsSuccessStatusCode)
                {
                    var createdTv = await response.Content.ReadFromJsonAsync<RequestTvDto>();
                    if (createdTv?.ID > 0)
                    {
                        _recentlyAddedIds.Add(createdTv.ID);
                        _logger.Info($"Successfully added TV with ID: {createdTv.ID}.");
                    }
                }
                else
                    _logger.Error($"Failed to add TV. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }

        private static async Task GetItemsByIdAsync(int count)
        {
            // var tasks = Enumerable.Range(_lastId - count + 1, count).Select(async i =>
            // {
            //     var response = await _httpClient.GetAsync(i.ToString());
            //     var tv = await response.Content.ReadFromJsonAsync<RequestTvDto>();
            //
            //     if (response.IsSuccessStatusCode)
            //         _logger.Info($"Successfully received TV with ID: {tv.ID}");
            //     else
            //         _logger.Error($"Failed to get TV. Status Code: {response.StatusCode}");
            // });
            
            var tasks = _recentlyAddedIds.Select(async i =>
            {
                var response = await _httpClient.GetAsync(i.ToString());
                var tv = await response.Content.ReadFromJsonAsync<RequestTvDto>();
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully received TV with ID: {tv.ID}");
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
            var tasks = _recentlyAddedIds.Select(async i =>
            {
                var updatedTV = new RequestTvDto
                {
                    ID = i,
                    Name = $"LG {i} updated",
                    Description = $"OLED {i} updated",
                    Size = 55,
                    Resolution = "2560x1440",
                    Frequency = 120,
                    ReleasedYear = 2024,
                    Price = 23700,
                    InStock = 7
                };
                
                var response = await _httpClient.PutAsJsonAsync(string.Empty, updatedTV);
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully updated TV with ID: {updatedTV.ID}");
                else
                    _logger.Error($"Failed to update TV. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
        
        private static async Task DeleteItemsAsync(int count)
        {
            var tasks = _recentlyAddedIds.Select(async i =>
            {
                var response = await _httpClient.DeleteAsync(i.ToString());
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully deleted TV with ID: {i}");
                else
                    _logger.Error($"Failed to delete TV. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
    }
}