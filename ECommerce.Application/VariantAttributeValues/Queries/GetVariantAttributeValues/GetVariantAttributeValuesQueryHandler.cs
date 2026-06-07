using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Application.VariantAttributeValues.Dtos;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.VariantAttributeValues.Queries.GetVariantAttributeValues
{
    public class GetVariantAttributeValuesQueryHandler(IVariantAttributeValueRepository _variantAttributeValueRepository,
                                                        IProductVariantRepository _productVariantRepository,
                                                        IAttributeValueRepository _attributeValueRepository,
                                                        IProductRepository _productRepository,
                                                        IUserContext _userContext) : IRequestHandler<GetVariantAttributeValuesQuery, IEnumerable<VariantAttributeValueDto>>
    {
        public async Task<IEnumerable<VariantAttributeValueDto>> Handle(GetVariantAttributeValuesQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            // Verify variant exists
            var productVariant = await _productVariantRepository.GetByIdAsync(request.ProductVariantId);
            if (productVariant == null)
                throw new Exception($"Product variant with ID {request.ProductVariantId} not found");

            // Verify user owns the product
            var product = await _productRepository.GetProductByIdAsync(p => p.Id == productVariant.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            // Get all variant attribute values
            var variantAttributeValues = await _variantAttributeValueRepository.GetByProductVariantIdAsync(request.ProductVariantId);

            // Map to DTOs with attribute value names
            var result = new List<VariantAttributeValueDto>();
            foreach (var vav in variantAttributeValues)
            {
                var attributeValue = await _attributeValueRepository.GetByIdAsync(vav.AttributeValueId);
                if (attributeValue != null)
                {
                    result.Add(new VariantAttributeValueDto
                    {
                        Id = vav.Id,
                        ProductVariantId = vav.ProductVariantId,
                        AttributeValueId = vav.AttributeValueId,
                        AttributeValueName = attributeValue.Value
                    });
                }
            }

            return result;
        }
    }
}
