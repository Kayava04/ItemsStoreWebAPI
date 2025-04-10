using System.Text;

namespace FileToolKit.Custom.IO.File.Tools
{
    public class CsvFileTool<T> : BaseFileTool<T> where T : class, new()
    {
        protected override IEnumerable<T> ParseContent(string content)
        {
            var rows = content.Split(Environment.NewLine)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(','))
                .ToArray();

            var headers = rows.First();
            var items = new List<T>();
            var props = typeof(T).GetProperties();

            foreach (var row in rows.Skip(1))
            {
                var item = new T();
                for (var i = 0; i < headers.Length && i < row.Length; i++)
                {
                    var prop = props.FirstOrDefault(p => p.Name.Equals(headers[i], StringComparison.OrdinalIgnoreCase));
                    if (prop == null)
                        continue;
                    
                    var value = Convert.ChangeType(row[i], prop.PropertyType);
                    prop.SetValue(item, value);
                }
                items.Add(item);
            }
            return items;
        }

        protected override string GenerateContent(IEnumerable<T> items)
        {
            var props = typeof(T).GetProperties();
            var sb = new StringBuilder();

            sb.AppendLine(string.Join(",", props.Select(p => p.Name)));

            foreach (var item in items)
                sb.AppendLine(string.Join(",", props.Select(p => p.GetValue(item))));

            return sb.ToString();
        }
    }
}