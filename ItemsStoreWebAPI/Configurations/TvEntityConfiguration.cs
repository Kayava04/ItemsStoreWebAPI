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
            
            builder.Property(tv => tv.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(tv => tv.Description)
                .HasColumnType("text")
                .IsRequired();
            
            builder.Property(tv => tv.Size)
                .IsRequired();
            
            builder.Property(tv => tv.Resolution)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Property(tv => tv.Frequency)
                .IsRequired();
            
            builder.Property(tv => tv.ReleasedYear)
                .IsRequired();
            
            builder.HasOne(tv => tv.StockItem)
                .WithOne()
                .HasForeignKey<TvEntity>(tv => tv.StockItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}