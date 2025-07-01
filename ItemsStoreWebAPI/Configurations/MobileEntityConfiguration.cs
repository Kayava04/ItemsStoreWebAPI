using ItemsStoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemsStoreWebAPI.Configurations
{
    public class MobileEntityConfiguration : IEntityTypeConfiguration<MobileEntity>
    {
        public void Configure(EntityTypeBuilder<MobileEntity> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.OS)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.ScreenSize)
                .IsRequired();

            builder.Property(m => m.BatteryCapacity)
                .IsRequired();

            builder.Property(m => m.RAM)
                .IsRequired();

            builder.Property(m => m.Storage)
                .IsRequired();

            builder.HasOne<StockItemEntity>()
                .WithOne()
                .HasForeignKey<MobileEntity>(m => m.StockItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}