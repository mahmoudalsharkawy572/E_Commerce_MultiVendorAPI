

namespace ECommerce.Domain.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal? PriceOverride { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Product Product { get; set; } = null!;
        public ICollection<VariantAttributeValue> VariantAttributeValues { get; set; } = new List<VariantAttributeValue>();
    }
}
