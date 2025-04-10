using FileToolKit.Custom.IO.File.Interfaces;
using FileToolKit.Custom.IO.File.Tools;

namespace FileToolKit.Custom.IO.File.Factories
{
    public class FileToolFactory<T> where T : class, new()
    {
        public IFileTool<T> GetTool(string extension)
        {
            return extension.ToLower() switch
            {
                ".csv" => new CsvFileTool<T>(),
                ".xlsx" => new ExcelFileTool<T>(),
                ".json" => new JsonFileTool<T>(),
                _ => throw new NotSupportedException($"Extension '{extension}' is not supported")
            };
        }
    }
}