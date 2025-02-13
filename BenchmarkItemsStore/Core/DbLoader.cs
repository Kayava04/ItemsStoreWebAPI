using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BenchmarkItemsStore.Core
{
    public class DbLoader
    {
        public static void InitializeDatabase()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var connectionString = configuration.GetConnectionString(nameof(LoggingDbContext));
            Console.WriteLine($"Database Connection String: {connectionString}");
            
            using var context = new LoggingDbContext(connectionString);
            context.Database.Migrate();
            context.SaveChanges();
        }
    }
}