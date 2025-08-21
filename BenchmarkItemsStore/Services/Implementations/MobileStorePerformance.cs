using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using BenchmarkItemsStore.Services.Interfaces;
using ItemsStoreWebAPI.Application.DTOs.Mobile;
using log4net;
using log4net.Config;

namespace BenchmarkItemsStore.Services.Implementations
{
    public class MobileStorePerformance : IStorePerformance
    {
        private readonly HttpClient _httpClient;
        private static int _lastId;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public MobileStorePerformance(string baseUrl)
        {
            _httpClient = new HttpClient(new SocketsHttpHandler { MaxConnectionsPerServer = 100 })
            {
                BaseAddress = new Uri(baseUrl)
            };
            
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config.xml"));
        }
        
        // Get last index of elements
        private async Task GetLastIdAsync()
        {
            var response = await _httpClient.GetAsync("filter");
            var mobiles = await response.Content.ReadFromJsonAsync<List<RequestMobileDto>>();
            
            _lastId = mobiles?.Any() == true ? mobiles.Max(m => m.Id) : 0;
        }
        
        // Generating range
        private static IEnumerable<int> GenerateRange(int lastId, int count)
        {
            var start = Math.Max(1, lastId - count + 1);
            return Enumerable.Range(start, count);
        }

        // Measuring execution time
        private async Task MeasureAsync(Func<Task> action, string name)
        {
            var timer = Stopwatch.StartNew();
            await action();
            timer.Stop();
            _logger.Info($"{name} finished. Total time: {timer.ElapsedMilliseconds} ms.");
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
            await MeasureAsync(() => AddItemsAsync(count), nameof(AddItemsAsync));
            
            await GetLastIdAsync();
            
            // Get all items timer
            await MeasureAsync(GetAllItemsAsync, nameof(GetAllItemsAsync));
            
            var ids = GenerateRange(_lastId, count).ToList();
            
            // Update items timer
            await MeasureAsync(() => UpdateItemsAsync(ids), nameof(UpdateItemsAsync));
            
            // Get items by ID timer
            await MeasureAsync(() => GetItemsByIdAsync(count), nameof(GetItemsByIdAsync));
            
            // Delete items timer
            await MeasureAsync(() => DeleteItemsAsync(ids), nameof(DeleteItemsAsync));
            
            await GetAllItemsAsync();
            
            // Add multiple items timer
            await MeasureAsync(() => AddMultipleItemsAsync(count), nameof(AddMultipleItemsAsync));
            
            await GetLastIdAsync();
            
            ids = GenerateRange(_lastId, count).ToList();
            
            // Update multiple items timer
            await MeasureAsync(() => UpdateMultipleItemsAsync(ids), nameof(UpdateMultipleItemsAsync));
            
            // Delete multiple items timer
            await MeasureAsync(() => DeleteMultipleItemsAsync(ids), nameof(DeleteMultipleItemsAsync));
            
            await GetAllItemsAsync();
            
            _logger.Info("Store performance tests completed.");
        }
        
        #region Single Methods
        
        private async Task AddItemsAsync(int count)
        {
            var tasks = Enumerable.Range(1, count).Select(async i =>
            {
                var mobile = new RequestMobileDto
                {
                    Name = $"IPhone {i}",
                    Description = $"16 Pro Max {i}",
                    OS = "IOS",
                    ScreenSize = 62,
                    BatteryCapacity = 100,
                    RAM = 16,
                    Storage = 256,
                    ReleasedYear = 2024,
                    Price = 54000,
                    InStock = 7
                };
                
                var response = await _httpClient.PostAsJsonAsync(string.Empty, mobile);

                if (response.IsSuccessStatusCode)
                {
                    var created = await response.Content.ReadFromJsonAsync<ResponseMobileDto>();
                    _logger.Info($"Successfully added Mobile with ID: {created?.Id}.");
                }
                else
                    _logger.Error($"Failed to add Mobile. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }

        private async Task GetItemsByIdAsync(int count)
        {
            var tasks = GenerateRange(_lastId, count).Select(async i =>
            {
                var response = await _httpClient.GetAsync(i.ToString());
                var mobile = await response.Content.ReadFromJsonAsync<RequestMobileDto>();
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully received Mobile with ID: {mobile?.Id}");
                else
                    _logger.Error($"Failed to get Mobile. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
        
        private async Task GetAllItemsAsync()
        {
            var response = await _httpClient.GetAsync("filter");
            var mobiles = await response.Content.ReadFromJsonAsync<List<RequestMobileDto>>();
            
            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully received all Mobiles. Total count: {mobiles?.Count}");
            else
                _logger.Error($"Failed to get all Mobiles. Status Code: {response.StatusCode}");
        }
        
        private async Task UpdateItemsAsync(IEnumerable<int> ids)
        {
            var tasks = ids.Select(async i =>
            {
                var updatedMobile = new RequestMobileDto
                {
                    Id = i,
                    Name = $"IPhone {i}",
                    Description = $"16 Pro Max {i}",
                    OS = "IOS",
                    ScreenSize = 62,
                    BatteryCapacity = 100,
                    RAM = 16,
                    Storage = 256,
                    ReleasedYear = 2024,
                    Price = 54000,
                    InStock = 7
                };
                
                var response = await _httpClient.PutAsJsonAsync(string.Empty, updatedMobile);
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully updated Mobile with ID: {updatedMobile.Id}");
                else
                    _logger.Error($"Failed to update Mobile. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
        
        private async Task DeleteItemsAsync(IEnumerable<int> ids)
        {
            var tasks = ids.Select(async i =>
            {
                var response = await _httpClient.DeleteAsync(i.ToString());
            
                if (response.IsSuccessStatusCode)
                    _logger.Info($"Successfully deleted Mobile with ID: {i}");
                else
                    _logger.Error($"Failed to delete Mobile. Status Code: {response.StatusCode}");
            });
            
            await Task.WhenAll(tasks);
        }
        
        #endregion

        #region Multiple Methods
        
        private async Task AddMultipleItemsAsync(int count)
        {
            var mobiles = Enumerable.Range(1, count).Select(i => new RequestMobileDto
            {
                Name = $"IPhone Multiple {i}",
                Description = $"16 Pro Max Multiple {i}",
                OS = "IOS",
                ScreenSize = 62,
                BatteryCapacity = 100,
                RAM = 16,
                Storage = 256,
                ReleasedYear = 2024,
                Price = 54000,
                InStock = 7
            }).ToList();

            var response = await _httpClient.PostAsJsonAsync("add-multiple", mobiles);

            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully added multiple Mobiles. Count: {count}");
            else
                _logger.Error($"Failed to add multiple Mobiles. Status Code: {response.StatusCode}");
        }
        
        private async Task UpdateMultipleItemsAsync(IEnumerable<int> ids)
        {
            var updatedMobiles = ids.Select(i => new RequestMobileDto
            {
                Id = i,
                Name = $"IPhone Updated Multiple {i}",
                Description = $"16 Pro Max Updated Multiple {i}",
                OS = "IOS",
                ScreenSize = 62,
                BatteryCapacity = 100,
                RAM = 16,
                Storage = 256,
                ReleasedYear = 2024,
                Price = 54000,
                InStock = 7
            }).ToList();

            var response = await _httpClient.PutAsJsonAsync("update-multiple", updatedMobiles);

            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully updated multiple Mobiles. Count: {updatedMobiles.Count}");
            else
                _logger.Error($"Failed to update multiple Mobiles. Status Code: {response.StatusCode}");
        }

        private async Task DeleteMultipleItemsAsync(IEnumerable<int> ids)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "delete-multiple")
            {
                Content = JsonContent.Create(ids)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                _logger.Info($"Successfully deleted multiple Mobiles. Count: {ids.ToList().Count}");
            else
                _logger.Error($"Failed to delete multiple Mobiles. Status Code: {response.StatusCode}");
        }

        #endregion
    }
}