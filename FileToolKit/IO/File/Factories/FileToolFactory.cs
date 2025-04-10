using FileToolKit.IO.File.Interfaces;
using FileToolKit.IO.File.Tools;

namespace FileToolKit.IO.File.Factories
{
    public class FileToolFactory<T>(
        CsvFileTool<T> csvTool,
        ExcelFileTool<T> excelTool,
        JsonFileTool<T> jsonTool) : IFileToolFactory<T> where T : class, new()
    {
        public IFileTool<T> GetTool(string extension)
        {
            return extension.ToLower() switch
            {
                ".csv" => csvTool,
                ".xlsx" => excelTool,
                ".json" => jsonTool,
                _ => throw new NotSupportedException($"Extension '{extension}' is not supported")
            };
        }
    }
}