using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configurations
{
    public class AttributeValueConfiguration : IEntityTypeConfiguration<AttributeValue>
    {
        public void Configure(EntityTypeBuilder<AttributeValue> builder)
        {
            builder.HasKey(av => av.Id);

            builder.Property(av => av.Value)
                   .IsRequired()
                   .HasMaxLength(512);

            // 1:N → VariantAttributeValues
            builder.HasMany(av => av.VariantAttributeValues)
                   .WithOne(vav => vav.AttributeValue)
                   .HasForeignKey(vav => vav.AttributeValueId)
                   .OnDelete(DeleteBehavior.NoAction); // avoid multiple cascade paths
        }
    }
}
