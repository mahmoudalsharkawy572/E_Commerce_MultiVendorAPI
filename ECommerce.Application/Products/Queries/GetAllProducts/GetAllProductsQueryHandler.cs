using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Products.Dtos;
using ECommerce.Application.Products.Queries.GetAllProducts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace E_commerce.Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler(IProductRepository _productRepository,
                                            IMapper _mapper, IUserContext _userContext)
                                            : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
    {
        public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if(user is null)
                throw new Exception("User Not found");

            var (products,totalCount) = await _productRepository.GetAllProductsAsync(p => p.Merchant.UserId == user.Id,request.SearchPhrase,request.PageSize,request.PageNumber);

            if (products == null)
                throw new Exception("No Products");

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            var pagedResult = new PagedResult<ProductDto>(productsDto, totalCount, request.PageSize, request.PageNumber);
            return pagedResult;
        }
    }
}
