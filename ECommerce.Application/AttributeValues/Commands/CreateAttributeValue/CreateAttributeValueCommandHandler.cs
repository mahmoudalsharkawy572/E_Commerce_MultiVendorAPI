using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.AttributeValues.Commands.CreateAttributeValue
{
    public class CreateAttributeValueCommandHandler(IAttributeValueRepository _attributeValueRepository,
                                                     IProductAttributeRepository _productAttributeRepository,
                                                     IUserContext _userContext,
                                                     IProductRepository _productRepository) : IRequestHandler<CreateAttributeValueCommand, int>
    {
        public async Task<int> Handle(CreateAttributeValueCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            var productAttribute = await _productAttributeRepository.GetByIdAsync(request.ProductAttributeId);
            if (productAttribute == null)
                throw new Exception($"Product attribute with ID {request.ProductAttributeId} not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == productAttribute.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            var existingValue = await _attributeValueRepository.ExistsAsync(request.ProductAttributeId, request.Value);
            if (existingValue)
                throw new Exception($"Attribute value '{request.Value}' already exists for this attribute");

            var attributeValue = new AttributeValue
            {
                ProductAttributeId = request.ProductAttributeId,
                Value = request.Value
            };

            await _attributeValueRepository.AddAsync(attributeValue);
            return attributeValue.Id;
        }
    }
}
