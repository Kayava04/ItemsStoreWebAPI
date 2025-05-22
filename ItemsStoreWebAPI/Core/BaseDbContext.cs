using ItemsStoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Core
{
    public abstract class BaseDbContext : DbContext
    {
        public DbSet<StockItemEntity> StockItems { get; set; }
        public DbSet<TvEntity> TVs { get; set; }
        
        protected BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}