using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.ProductVariants.Commands.UpdateProductVariant
{
    public class UpdateProductVariantCommandHandler(IProductVariantRepository _productVariantRepository,
                                                     IProductRepository _productRepository,
                                                     IUserContext _userContext) : IRequestHandler<UpdateProductVariantCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
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

            var existingSku = await _productVariantRepository.GetBySkuAsync(request.SKU);
            if (existingSku != null && existingSku.Id != request.Id)
                throw new Exception($"SKU '{request.SKU}' already exists");

            productVariant.SKU = request.SKU;
            productVariant.Quantity = request.Quantity;
            productVariant.PriceOverride = request.PriceOverride;
            productVariant.IsActive = request.IsActive;
            productVariant.UpdatedAt = DateTime.UtcNow;

            await _productVariantRepository.UpdateAsync(productVariant);
            return Unit.Value;
        }
    }
}
