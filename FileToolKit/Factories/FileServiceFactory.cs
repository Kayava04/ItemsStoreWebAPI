using FileToolKit.Interfaces;
using FileToolKit.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileToolKit.Factories
{
    public class FileServiceFactory<T>(IServiceProvider serviceProvider)
        : IFileServiceFactory<T>where T : class, new()
    {
        public IFileHandler<T> GetService(string? fileType = null)
        {
            return fileType.ToLower() switch
            {
                "csv" => serviceProvider.GetRequiredService<CsvFileService<T>>(),
                _ => throw new ArgumentException("Unsupported file type.")
            };
        }
    }
}