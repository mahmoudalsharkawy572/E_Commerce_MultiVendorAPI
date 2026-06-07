using ECommerce.Application.Common;
using ECommerce.Application.Products.Dtos;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public string? SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
