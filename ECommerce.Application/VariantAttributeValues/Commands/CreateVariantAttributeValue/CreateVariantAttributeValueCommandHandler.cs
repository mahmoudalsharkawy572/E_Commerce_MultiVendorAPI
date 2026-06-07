using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.VariantAttributeValues.Commands.CreateVariantAttributeValue
{
    public class CreateVariantAttributeValueCommandHandler(IVariantAttributeValueRepository _variantAttributeValueRepository,
                                                            IProductVariantRepository _productVariantRepository,
                                                            IProductAttributeRepository _productAttributeRepository,
                                                            IAttributeValueRepository _attributeValueRepository,
                                                            IProductRepository _productRepository,
                                                            IUserContext _userContext) : IRequestHandler<CreateVariantAttributeValueCommand, int>
    {
        public async Task<int> Handle(CreateVariantAttributeValueCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            // Verify variant exists
            var productVariant = await _productVariantRepository.GetByIdAsync(request.ProductVariantId);
            if (productVariant == null)
                throw new Exception($"Product variant with ID {request.ProductVariantId} not found");

            // Verify product exists and user owns it
            var product = await _productRepository.GetProductByIdAsync(p => p.Id == productVariant.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            // Verify attribute value exists
            var attributeValue = await _attributeValueRepository.GetByIdAsync(request.AttributeValueId);
            if (attributeValue == null)
                throw new Exception($"Attribute value with ID {request.AttributeValueId} not found");

            // Verify attribute value belongs to an attribute of the same product
            var productAttribute = await _productAttributeRepository.GetByIdAsync(attributeValue.ProductAttributeId);
            if (productAttribute == null || productAttribute.ProductId != product.Id)
                throw new Exception("Attribute value does not belong to this product");

            // Check if this variant-attribute combination already exists
            var existingLink = await _variantAttributeValueRepository.ExistsAsync(request.ProductVariantId, request.AttributeValueId);
            if (existingLink)
                throw new Exception("This attribute value is already assigned to this variant");

            // Create the link
            var variantAttributeValue = new VariantAttributeValue
            {
                ProductVariantId = request.ProductVariantId,
                AttributeValueId = request.AttributeValueId
            };

            await _variantAttributeValueRepository.AddAsync(variantAttributeValue);
            return variantAttributeValue.Id;
        }
    }
}
