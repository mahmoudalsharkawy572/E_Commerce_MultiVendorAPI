using ECommerce.Application.ProductVariants.Dtos;
using MediatR;

namespace ECommerce.Application.ProductVariants.Queries.GetProductVariantsForProduct
{
    public class GetProductVariantsForProductQuery(int productId) : IRequest<IEnumerable<ProductVariantDto>>
    {
        public int ProductId { get; set; } = productId;
    }
}
