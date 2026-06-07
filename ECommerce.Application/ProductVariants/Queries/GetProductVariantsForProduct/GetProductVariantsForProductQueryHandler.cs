using AutoMapper;
using ECommerce.Application.Contracts;
using ECommerce.Application.ProductVariants.Dtos;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.ProductVariants.Queries.GetProductVariantsForProduct
{
    public class GetProductVariantsForProductQueryHandler(IMapper _mapper,
                                                          IProductVariantRepository _productVariantRepository,
                                                          IProductRepository _productRepository,
                                                          IUserContext _userContext) : IRequestHandler<GetProductVariantsForProductQuery, IEnumerable<ProductVariantDto>>
    {
        public async Task<IEnumerable<ProductVariantDto>> Handle(GetProductVariantsForProductQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == request.ProductId);
            if (product == null)
                throw new Exception($"Product with ID {request.ProductId} not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            var productVariants = await _productVariantRepository.GetByProductIdAsync(request.ProductId);

            var productVariantDtos = _mapper.Map<IEnumerable<ProductVariantDto>>(productVariants);
            return productVariantDtos;
        }
    }
}
