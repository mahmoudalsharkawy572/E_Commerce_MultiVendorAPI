using AutoMapper;
using ECommerce.Application.Contracts;
using ECommerce.Application.ProductVariants.Dtos;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.ProductVariants.Queries.GetProductVariantById
{
    public class GetProductVariantByIdQueryHandler(IMapper _mapper,
                                                    IProductVariantRepository _productVariantRepository,
                                                    IProductRepository _productRepository,
                                                    IUserContext _userContext) : IRequestHandler<GetProductVariantByIdQuery, ProductVariantDto>
    {
        public async Task<ProductVariantDto> Handle(GetProductVariantByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            var productVariant = await _productVariantRepository.GetByIdAsync(request.Id);
            if (productVariant == null)
                throw new Exception($"Product variant with ID {request.Id} not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == productVariant.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            var productVariantDto = _mapper.Map<ProductVariantDto>(productVariant);
            return productVariantDto;
        }
    }
}
