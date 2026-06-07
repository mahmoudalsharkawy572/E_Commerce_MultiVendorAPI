using ECommerce.Application.AttributeValues.Dtos;
using MediatR;

namespace ECommerce.Application.AttributeValues.Queries.GetAttributeValuesForAttribute
{
    public class GetAttributeValuesForAttributeQuery(int productAttributeId) : IRequest<IEnumerable<AttributeValueDto>>
    {
        public int ProductAttributeId { get; set; } = productAttributeId;
    }
}
