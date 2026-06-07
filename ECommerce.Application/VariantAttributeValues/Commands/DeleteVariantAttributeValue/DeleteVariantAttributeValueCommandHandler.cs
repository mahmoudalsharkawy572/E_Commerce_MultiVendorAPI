using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.VariantAttributeValues.Commands.DeleteVariantAttributeValue
{
    public class DeleteVariantAttributeValueCommandHandler(IVariantAttributeValueRepository _variantAttributeValueRepository,
                                                            IProductVariantRepository _productVariantRepository,
                                                            IProductRepository _productRepository,
                                                            IUserContext _userContext) : IRequestHandler<DeleteVariantAttributeValueCommand>
    {
        public async Task Handle(DeleteVariantAttributeValueCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            // This is tricky - we need to fetch the VariantAttributeValue to get ProductVariantId
            // For now, we'll need to modify the repository or add a new method
            // For this example, we'll assume the repository can be enhanced

            var variantAttributeValue = await _variantAttributeValueRepository.GetByIdAsync(request.Id);
            if (variantAttributeValue == null)
                throw new Exception("Variant Attribute Value not found");

            var productVariant = await _productVariantRepository.GetByIdAsync(variantAttributeValue.ProductVariantId);
            if (productVariant == null)
                throw new Exception("Product Variant not found");

            var product = await _productRepository.GetProductByIdAsync( p => p.Id == productVariant.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized to delete this variant attribute value");

            await _variantAttributeValueRepository.DeleteAsync(request.Id);
        }
    }
}
