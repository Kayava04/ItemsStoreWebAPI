using FileToolKit.Custom.IO.File.Factories;
using FileToolKit.Custom.IO.File.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace FileToolKit.Custom.IO.File.Extensions
{
    public static class FileToolKitExtensions
    {
        public static IServiceCollection AddFileTools<T>(this IServiceCollection services) where T : class, new()
        {
            services.AddScoped<CsvFileTool<T>>();
            services.AddScoped<ExcelFileTool<T>>();
            services.AddScoped<JsonFileTool<T>>();
            services.AddScoped<FileToolFactory<T>>();
            
            return services;
        }
    }
}