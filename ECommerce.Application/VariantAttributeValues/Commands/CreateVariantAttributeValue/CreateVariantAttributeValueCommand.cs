using MediatR;

namespace ECommerce.Application.VariantAttributeValues.Commands.CreateVariantAttributeValue
{
    public class CreateVariantAttributeValueCommand : IRequest<int>
    {
        public int ProductVariantId { get; set; }
        public int AttributeValueId { get; set; }
    }
}
