using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.ProductVariants.Commands.DeleteProductVariant
{
    public class DeleteProductVariantCommandHandler(IProductVariantRepository _productVariantRepository,
                                                     IProductRepository _productRepository,
                                                     IUserContext _userContext) : IRequestHandler<DeleteProductVariantCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
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

            await _productVariantRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
