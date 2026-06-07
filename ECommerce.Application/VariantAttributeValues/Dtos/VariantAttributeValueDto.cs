namespace ECommerce.Application.VariantAttributeValues.Dtos
{
    public class VariantAttributeValueDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int AttributeValueId { get; set; }
        public string AttributeValueName { get; set; } = default!;
    }
}
