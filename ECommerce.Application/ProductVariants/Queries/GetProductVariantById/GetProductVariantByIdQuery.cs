using ECommerce.Application.ProductVariants.Dtos;
using MediatR;

namespace ECommerce.Application.ProductVariants.Queries.GetProductVariantById
{
    public class GetProductVariantByIdQuery(int id) : IRequest<ProductVariantDto>
    {
        public int Id { get; set; } = id;
    }
}
