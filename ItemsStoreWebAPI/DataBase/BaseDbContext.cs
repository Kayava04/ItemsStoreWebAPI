using ItemsStoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.DataBase
{
    public abstract class BaseDbContext(DbContextOptions options)
        : DbContext(options)
    {
        public DbSet<StockItemEntity> StockItems { get; set; }
        public DbSet<TvEntity> TVs { get; set; }
        public DbSet<MobileEntity> Mobiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}