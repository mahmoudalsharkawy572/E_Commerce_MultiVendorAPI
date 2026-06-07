using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configurations
{
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(m => m.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.UpdatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // 1:N → Products
            builder.HasMany(m => m.Products)
                   .WithOne(p => p.Merchant)
                   .HasForeignKey(p => p.MerchantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
