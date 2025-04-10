using FileToolKit.IO.File.Interfaces;

namespace FileToolKit.IO.File.Tools
{
    public abstract class BaseFileTool<T> : IFileTool<T> where T : class, new()
    {
        protected abstract IEnumerable<T> ParseContent(string content);
        protected abstract string GenerateContent(IEnumerable<T> items);
        
        public virtual async Task<IEnumerable<T>> ImportAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();
            return ParseContent(content);
        }

        public virtual async Task<byte[]> ExportAsync(IEnumerable<T> items)
        {
            var content = GenerateContent(items);
            await using var ms = new MemoryStream();
            await using var writer = new StreamWriter(ms);
            await writer.WriteAsync(content);
            await writer.FlushAsync();
            return ms.ToArray();
        }
    }
}