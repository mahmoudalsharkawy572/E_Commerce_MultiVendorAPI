using ECommerce.Application.AttributeValues.Dtos;
using MediatR;

namespace ECommerce.Application.AttributeValues.Commands.CreateAttributeValue
{
    public class CreateAttributeValueCommand : IRequest<int>
    {
        public int ProductAttributeId { get; set; }
        public string Value { get; set; } = default!;
    }
}
