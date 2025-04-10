using System.Text.Json;

namespace FileToolKit.Custom.IO.File.Tools
{
    public class JsonFileTool<T> : BaseFileTool<T> where T : class, new()
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        
        protected override IEnumerable<T> ParseContent(string content)
        {
            return JsonSerializer.Deserialize<List<T>>(content, _options)
                   ?? Enumerable.Empty<T>();
        }

        protected override string GenerateContent(IEnumerable<T> items)
        {
            return JsonSerializer.Serialize(items, _options);
        }
    }
}