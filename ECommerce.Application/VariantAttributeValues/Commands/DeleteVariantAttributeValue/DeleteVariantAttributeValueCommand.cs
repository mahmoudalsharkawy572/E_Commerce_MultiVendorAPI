using MediatR;

namespace ECommerce.Application.VariantAttributeValues.Commands.DeleteVariantAttributeValue
{
    public class DeleteVariantAttributeValueCommand(int id) : IRequest
    {
        public int Id { get; set; } = id;
    }
}
