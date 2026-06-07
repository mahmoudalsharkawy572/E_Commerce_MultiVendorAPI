using ECommerce.Application.ProductAttributes.Dtos;
using MediatR;

namespace ECommerce.Application.ProductAttributes.Queries.GetAttributesForProductById
{
    public class GetAttributesForProductByIdQuery(int productId) : IRequest<IEnumerable<ProductAttributeDto>>
    {
        public int ProductId { get; set; } = productId;
    }
}
