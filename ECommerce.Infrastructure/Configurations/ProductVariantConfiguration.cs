using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasKey(pv => pv.Id);

            builder.Property(pv => pv.SKU)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(pv => pv.SKU)
                   .IsUnique();

            builder.Property(pv => pv.Quantity)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(pv => pv.PriceOverride)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired(false);

            builder.Property(pv => pv.IsActive)
                   .HasDefaultValue(true);

            builder.Property(pv => pv.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(pv => pv.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // 1:N → VariantAttributeValues
            builder.HasMany(pv => pv.VariantAttributeValues)
                   .WithOne(vav => vav.ProductVariant)
                   .HasForeignKey(vav => vav.ProductVariantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
