using AutoMapper;
using ECommerce.Application.AttributeValues.Dtos;
using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.AttributeValues.Queries.GetAttributeValueById
{
    public class GetAttributeValueByIdQueryHandler(IMapper _mapper,
                                                    IAttributeValueRepository _attributeValueRepository,
                                                    IProductAttributeRepository _productAttributeRepository,
                                                    IUserContext _userContext,
                                                    IProductRepository _productRepository) : IRequestHandler<GetAttributeValueByIdQuery, AttributeValueDto>
    {
        public async Task<AttributeValueDto> Handle(GetAttributeValueByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            var attributeValue = await _attributeValueRepository.GetByIdAsync(request.Id);
            if (attributeValue == null)
                throw new Exception($"Attribute value with ID {request.Id} not found");

            var productAttribute = await _productAttributeRepository.GetByIdAsync(attributeValue.ProductAttributeId);
            if (productAttribute == null)
                throw new Exception("Product attribute not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == productAttribute.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            var attributeValueDto = _mapper.Map<AttributeValueDto>(attributeValue);
            return attributeValueDto;
        }
    }
}
