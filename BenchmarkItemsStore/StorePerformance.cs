using System.Diagnostics;
using System.Net.Http.Json;
using BenchmarkItemsStore.Core;
using BenchmarkItemsStore.Entities;
using ItemsStoreWebAPI.Models;
using Microsoft.Extensions.Logging;


namespace BenchmarkItemsStore
{
    public class StorePerformance
    {
        private static HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5117/v1/stock/electronic/tv/")
        };
        
        private const string STATUS_SUCCESS = "Success";
        private const string STATUS_FAILURE = "Failure";
        
        private readonly ILogger<StorePerformance> _logger;
        private readonly LoggingDbContext _loggingDbContext;

        public StorePerformance(ILogger<StorePerformance> logger)
        {
            _logger = logger;
        }
        
        public async Task RunAsync(int tvCount = 0, int id = 0)
        {
            _logger.LogInformation($"Loading store performance...");

            await AddItemsAsync(tvCount);
            // var GetItemsByIdTimer = await GetItemsByIdAsync(id);
            // var GetAllItemsTimer = await GetAllItemsAsync();
            // var UpdateItemsTimer = await UpdateItemsAsync();
            // var DeleteItemsTimer = await DeleteItemsAsync(id);
            
            // _logger.LogInformation($"Store performance completed. Total time: {AddItemsTimer} ms");
        }
        
        //TODO: Change log in every method
        //      Identify why first try took more time than others
        //      Look up for consuming
        //      Check how to descrease time to sending all http requests

        private async Task LogToDatabase(long timeStamp, string method, string status, string message)
        {
            var log = new LogEntity
            {
                Timestamp = timeStamp,
                Method = method,
                Status = status,
                Message = message
            };
            
            _loggingDbContext.Logs.Add(log);
            await _loggingDbContext.SaveChangesAsync();
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

                var time = Stopwatch.StartNew();
                
                var response = await _httpClient.PostAsJsonAsync(String.Empty, tv);

                time.Stop();

                if (response.IsSuccessStatusCode)
                {
                    var message = $"Successfully added TV {i}";
                    _logger.LogInformation(message);
                    await LogToDatabase(time.ElapsedMilliseconds, nameof(AddItemsAsync), STATUS_SUCCESS, message);
                }
                else
                {
                    var message = $"Failed to add TV {i}. Status Code: {response.StatusCode}";
                    _logger.LogError(message);
                    await LogToDatabase(time.ElapsedMilliseconds, nameof(AddItemsAsync), STATUS_FAILURE, message);
                }
            }
        }
        
        public async Task<long> GetItemsByIdAsync(int id)
        {
            var time = Stopwatch.StartNew();

            var response = await _httpClient.GetAsync(id.ToString());
            var tv = await response.Content.ReadFromJsonAsync<TV>();
            
            if (response.IsSuccessStatusCode)
                _logger.LogInformation($"Successfully received TV with ID: {tv.ID}");
            else
                _logger.LogError($"Failed to get TV. Status Code: {response.StatusCode}");
                
            time.Stop();
            return time.ElapsedMilliseconds;
        }
        
        public async Task<long> GetAllItemsAsync()
        {
            var time = Stopwatch.StartNew();
            
            var response = await _httpClient.GetAsync(String.Empty);
            var tvs = await response.Content.ReadFromJsonAsync<List<TV>>();
            
            if (response.IsSuccessStatusCode)
                _logger.LogInformation($"Successfully received all TVs. Total count: {tvs.Count}");
            else
                _logger.LogError($"Failed to get all TVs. Status Code: {response.StatusCode}");
            
            time.Stop();
            return time.ElapsedMilliseconds;
        }

        public async Task<long> UpdateItemsAsync()
        {
            var time = Stopwatch.StartNew();
            
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
                _logger.LogInformation($"Successfully updated TV with ID: {updatedTV.ID}");
            else
                _logger.LogError($"Failed to update TV. Status Code: {response.StatusCode}");
            
            time.Stop();
            return time.ElapsedMilliseconds;
        }

        public async Task<long> DeleteItemsAsync(int id)
        {
            var time = Stopwatch.StartNew();
            
            var response = await _httpClient.DeleteAsync(id.ToString());
            
            if (response.IsSuccessStatusCode)
                _logger.LogInformation($"Successfully deleted TV with ID: {id}");
            else
                _logger.LogError($"Failed to delete TV. Status Code: {response.StatusCode}");
            
            time.Stop();
            return time.ElapsedMilliseconds;
        }
    }
}