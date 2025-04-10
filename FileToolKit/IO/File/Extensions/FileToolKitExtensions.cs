using FileToolKit.IO.File.Factories;
using FileToolKit.IO.File.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace FileToolKit.IO.File.Extensions
{
    public static class FileToolKitExtensions
    {
        private static IServiceCollection AddFileTools<T>(this IServiceCollection services) where T : class, new()
        {
            services.AddScoped<CsvFileTool<T>>();
            services.AddScoped<ExcelFileTool<T>>();
            services.AddScoped<JsonFileTool<T>>();
            services.AddScoped<IFileToolFactory<T>, FileToolFactory<T>>();
            
            return services;
        }
        
        public static IServiceCollection AddFileToolKitFor<T>(this IServiceCollection services) where T : class, new()
        {
            return services.AddFileTools<T>();
        }
    }
}