using System.Text;

namespace FileToolKit.Services
{
    public class CsvFileService<T> : FileService<T> where T : class, new()
    {
        public override async Task<IEnumerable<T>> ReadDataAsync(Stream dataStream)
        {
            using var reader = new StreamReader(dataStream);
            var lines = await reader.ReadToEndAsync();
            var rows = lines.Split(Environment.NewLine)
                .Select(line => line.Split(','))
                .Where(row => row.Length > 0)
                .ToArray();

            return ParseData(rows);
        }

        public override Task<byte[]> WriteDataAsync(IEnumerable<T> items)
        {
            var rows = ConvertToRows(items);
            var csvData = string.Join(Environment.NewLine, rows.Select(row => string.Join(",", row)));
            return Task.FromResult(Encoding.UTF8.GetBytes(csvData));
        }
    }
}