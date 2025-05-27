using ItemsStoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Core
{
    public class PostgresDbContext : BaseDbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockItemEntity>()
                .Property(si => si.AddedAt)
                .HasDefaultValueSql("NOW()");
            
            base.OnModelCreating(modelBuilder);
        }
    }
}