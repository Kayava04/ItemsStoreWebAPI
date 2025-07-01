using ItemsStoreWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemsStoreWebAPI.Configurations
{
    public class TvEntityConfiguration : IEntityTypeConfiguration<TvEntity>
    {
        public void Configure(EntityTypeBuilder<TvEntity> builder)
        {
            builder.HasKey(tv => tv.Id);

            builder.Property(tv => tv.ScreenSize)
                .IsRequired();

            builder.Property(tv => tv.Resolution)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(tv => tv.Frequency)
                .IsRequired();

            builder.HasOne(tv => tv.StockItem)
                .WithOne()
                .HasForeignKey<TvEntity>(tv => tv.StockItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}