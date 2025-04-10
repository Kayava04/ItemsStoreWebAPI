using ClosedXML.Excel;

namespace FileToolKit.Custom.IO.File.Tools
{
    public class ExcelFileTool<T> : BaseFileTool<T> where T : class, new()
    {
        public override Task<IEnumerable<T>> ImportAsync(Stream stream)
        {
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var headers = worksheet.Row(1).Cells().Select(c => c.Value.ToString()).ToArray();
            var properties = typeof(T).GetProperties();
            var items = new List<T>();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var item = new T();

                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    var cell = row.Cell(i + 1).GetValue<string>();
                    var property = properties.FirstOrDefault(p => 
                        p.Name.Equals(header, StringComparison.OrdinalIgnoreCase));

                    if (property == null || string.IsNullOrEmpty(cell))
                        continue;
                    
                    var value = Convert.ChangeType(cell, property.PropertyType);
                    property.SetValue(item, value);
                }
                items.Add(item);
            }
            return Task.FromResult(items.AsEnumerable());
        }

        public override Task<byte[]> ExportAsync(IEnumerable<T> items)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            var props = typeof(T).GetProperties();

            for (var i = 0; i < props.Length; i++)
                worksheet.Cell(1, i + 1).Value = props[i].Name;

            var rowIndex = 2;
            foreach (var item in items)
            {
                for (var colIndex = 0; colIndex < props.Length; colIndex++)
                {
                    var value = props[colIndex].GetValue(item)?.ToString() ?? "";
                    worksheet.Cell(rowIndex, colIndex + 1).Value = value;
                }
                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            
            return Task.FromResult(ms.ToArray());
        }
        
        protected override IEnumerable<T> ParseContent(string content)
        {
            throw new NotSupportedException("ExcelFileTool does not support text parsing");
        }

        protected override string GenerateContent(IEnumerable<T> items)
        {
            throw new NotSupportedException("ExcelFileTool does not support text generation");
        }
    }
}