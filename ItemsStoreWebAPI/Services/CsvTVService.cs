using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Services
{
    public class CsvTVService : ICsvService<TV>
    {
        private readonly CsvConfiguration _csvConfig;

        public CsvTVService()
        {
            _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null
            };
        }
        
        public async Task<IEnumerable<TV>> ImportFromCsvAsync(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, _csvConfig);

            var records = csv.GetRecordsAsync<TV>();
            var tvs = new List<TV>();

            await foreach (var tv in records)
            {
                tv.AddedAt = DateTime.UtcNow;
                tvs.Add(tv);
            }
            
            return tvs;
        }

        public async Task<byte[]> ExportToCsvAsync(IEnumerable<TV> tvs)
        {
            await using var memoryStream = new MemoryStream();
            await using var writer = new StreamWriter(memoryStream);
            await using var csv = new CsvWriter(writer, _csvConfig);
            
            await csv.WriteRecordsAsync(tvs);
            await writer.FlushAsync();
            
            return memoryStream.ToArray();
        }
    }
}