using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.Name)
                   .IsRequired()
                   .HasMaxLength(256);

            // 1:N → AttributeValues
            builder.HasMany(pa => pa.Values)
                   .WithOne(av => av.ProductAttribute)
                   .HasForeignKey(av => av.ProductAttributeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
