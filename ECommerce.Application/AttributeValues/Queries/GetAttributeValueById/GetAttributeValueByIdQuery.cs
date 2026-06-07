using ECommerce.Application.AttributeValues.Dtos;
using MediatR;

namespace ECommerce.Application.AttributeValues.Queries.GetAttributeValueById
{
    public class GetAttributeValueByIdQuery(int id) : IRequest<AttributeValueDto>
    {
        public int Id { get; set; } = id;
    }
}
