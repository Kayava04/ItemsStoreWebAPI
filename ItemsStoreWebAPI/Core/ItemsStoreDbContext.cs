using ItemsStoreWebAPI.Configurations;
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
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemsStoreDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new TvEntityConfiguration());

            modelBuilder.Entity<StockItemEntity>(entity =>
            {
                entity.HasKey(si => si.Id);

                entity.Property(si => si.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(si => si.InStock)
                    .IsRequired();

                entity.Property(si => si.ModifiedAt)
                    .IsRequired();

                var provider = Database.ProviderName;
                
                if (provider != null && provider.Contains("SqlServer"))
                    entity.Property(si => si.AddedAt)
                        .HasDefaultValueSql("GETDATE()");
                else if (provider != null && provider.Contains("Npgsql"))
                    entity.Property(si => si.AddedAt)
                        .HasDefaultValueSql("NOW()");
                else
                    throw new NotSupportedException($"Database provider {provider} is not supported.");
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}