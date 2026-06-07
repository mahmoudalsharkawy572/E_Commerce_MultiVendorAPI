namespace ECommerce.Domain.Models
{
    public class AttributeValue
    {
        public int Id { get; set; }
        public int ProductAttributeId { get; set; }
        public string Value { get; set; } = default!;

        // Navigation
        public ProductAttribute ProductAttribute { get; set; } = null!;
        public ICollection<VariantAttributeValue> VariantAttributeValues { get; set; } = new List<VariantAttributeValue>();
    }
}
