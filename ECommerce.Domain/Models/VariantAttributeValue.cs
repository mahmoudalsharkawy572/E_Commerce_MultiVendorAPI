

namespace ECommerce.Domain.Models
{
    public class VariantAttributeValue
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int AttributeValueId { get; set; }

        // Navigation
        public ProductVariant ProductVariant { get; set; } = null!;
        public AttributeValue AttributeValue { get; set; } = null!; 
    }
}
