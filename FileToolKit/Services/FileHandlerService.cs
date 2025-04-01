using System.Reflection;
using FileToolKit.Interfaces;

namespace FileToolKit.Services
{
    public abstract class FileService<T> : IFileHandler<T> where T : class, new()
    {
        public abstract Task<IEnumerable<T>> ReadDataAsync(Stream dataStream);
        public abstract Task<byte[]> WriteDataAsync(IEnumerable<T> items);

        protected IEnumerable<T> ParseData(IEnumerable<string[]> rows)
        {
            var items = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var headers = rows.FirstOrDefault();

            foreach (var row in rows.Skip(1))
            {
                var item = new T();
                for (var i = 0; i < headers.Length; i++)
                {
                    var property = properties.FirstOrDefault(p => p.Name.Equals(headers[i], StringComparison.InvariantCultureIgnoreCase));
                    if (property == null) continue;
                    var value = Convert.ChangeType(row[i], property.PropertyType);
                    property.SetValue(item, value);
                }
                items.Add(item);
            }
            return items;
        }

        protected IEnumerable<string[]> ConvertToRows(IEnumerable<T> items)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var rows = new List<string[]>();
            
            var headers = properties.Select(p => p.Name).ToArray();
            rows.Add(headers);

            rows.AddRange(items.Select(item => properties.Select(p => p.GetValue(item)?.ToString() ?? string.Empty).ToArray()));
            return rows;
        }
    }
}