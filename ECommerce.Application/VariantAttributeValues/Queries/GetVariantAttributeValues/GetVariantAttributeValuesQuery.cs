using ECommerce.Application.VariantAttributeValues.Dtos;
using MediatR;

namespace ECommerce.Application.VariantAttributeValues.Queries.GetVariantAttributeValues
{
    public class GetVariantAttributeValuesQuery(int productVariantId) : IRequest<IEnumerable<VariantAttributeValueDto>>
    {
        public int ProductVariantId { get; set; } = productVariantId;
    }
}
