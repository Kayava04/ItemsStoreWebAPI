using ItemsStoreWebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemsStoreWebAPI.DataAccess.Configurations
{
    public class StockItemConfiguration : IEntityTypeConfiguration<StockItemEntity>
    {
        public void Configure(EntityTypeBuilder<StockItemEntity> builder)
        {
            builder.HasKey(si => si.Id);
            
            builder.Property(si => si.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(si => si.Description)
                .IsRequired()
                .HasColumnType("text");
            
            builder.Property(si => si.ReleasedYear)
                .IsRequired();
            
            builder.Property(si => si.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();
            
            builder.Property(si => si.InStock)
                .IsRequired();

            builder.Property(si => si.AddedAt)
                .HasDefaultValueSql("GETDATE()");
            
            builder.Property(si => si.ModifiedAt)
                .IsRequired();
        }
    }
}