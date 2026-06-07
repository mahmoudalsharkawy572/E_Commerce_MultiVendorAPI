using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    public class VariantAttributeValueConfiguration : IEntityTypeConfiguration<VariantAttributeValue>
    {
        public void Configure(EntityTypeBuilder<VariantAttributeValue> builder)
        {
            builder.HasKey(vav => vav.Id);

            // Prevent duplicate combinations
            builder.HasIndex(vav => new { vav.ProductVariantId, vav.AttributeValueId })
                   .IsUnique();
        }
    }
}
