using ItemsStoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Core
{
    public class ItemsStoreDbContext(DbContextOptions<ItemsStoreDbContext> options)
        : DbContext(options)
    {
        public DbSet<StockItemEntity> StockItems { get; set; }
        public DbSet<TvEntity> TVs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemsStoreDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}