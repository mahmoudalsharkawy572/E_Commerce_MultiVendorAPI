using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.ProductVariants.Commands.CreateProductVariant
{
    public class CreateProductVariantCommandHandler(IProductVariantRepository _productVariantRepository,
                                                     IProductRepository _productRepository,
                                                     IUserContext _userContext) : IRequestHandler<CreateProductVariantCommand, int>
    {
        public async Task<int> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == request.ProductId);
            if (product == null)
                throw new Exception($"Product with ID {request.ProductId} not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            var existingSku = await _productVariantRepository.GetBySkuAsync(request.SKU);
            if (existingSku != null)
                throw new Exception($"SKU '{request.SKU}' already exists");

            var productVariant = new ProductVariant
            {
                ProductId = request.ProductId,
                SKU = request.SKU,
                Quantity = request.Quantity,
                PriceOverride = request.PriceOverride,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _productVariantRepository.AddAsync(productVariant);
            return productVariant.Id;
        }
    }
}
