using BenchmarkItemsStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace BenchmarkItemsStore.Core
{
    public class LoggingDbContext : DbContext
    {
        private readonly string _connectionString;
        
        public LoggingDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public DbSet<LogEntity> Logs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntity>().ToTable("logs");

            base.OnModelCreating(modelBuilder);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}