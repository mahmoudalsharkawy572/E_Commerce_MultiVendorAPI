namespace ECommerce.Application.AttributeValues.Dtos
{
    public class AttributeValueDto
    {
        public int Id { get; set; }
        public int ProductAttributeId { get; set; }
        public string Value { get; set; } = default!;
    }
}
