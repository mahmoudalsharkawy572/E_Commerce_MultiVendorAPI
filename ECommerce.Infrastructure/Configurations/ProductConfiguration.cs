using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(p => p.Description)
                   .HasMaxLength(2000);

            builder.Property(p => p.Status)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasDefaultValue("Active");

            builder.Property(p => p.BasePrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // 1:N → ProductAttributes
            builder.HasMany(p => p.Attributes)
                   .WithOne(pa => pa.Product)
                   .HasForeignKey(pa => pa.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 1:N → ProductVariants
            builder.HasMany(p => p.Variants)
                   .WithOne(pv => pv.Product)
                   .HasForeignKey(pv => pv.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
