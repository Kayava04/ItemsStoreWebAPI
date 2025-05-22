using ItemsStoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemsStoreWebAPI.Core
{
    public class SqlServerDbContext : BaseDbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockItemEntity>()
                .Property(si => si.AddedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}