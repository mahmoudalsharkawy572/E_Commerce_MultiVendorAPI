using AutoMapper;
using ECommerce.Application.AttributeValues.Dtos;
using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.AttributeValues.Queries.GetAttributeValuesForAttribute
{
    public class GetAttributeValuesForAttributeQueryHandler(IMapper _mapper,
                                                            IAttributeValueRepository _attributeValueRepository,
                                                            IProductAttributeRepository _productAttributeRepository,
                                                            IUserContext _userContext,
                                                            IProductRepository _productRepository) : IRequestHandler<GetAttributeValuesForAttributeQuery, IEnumerable<AttributeValueDto>>
    {
        public async Task<IEnumerable<AttributeValueDto>> Handle(GetAttributeValuesForAttributeQuery request, CancellationToken cancellationToken)
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

            var attributeValues = await _attributeValueRepository.GetByProductAttributeIdAsync(request.ProductAttributeId);

            var attributeValueDtos = _mapper.Map<IEnumerable<AttributeValueDto>>(attributeValues);
            return attributeValueDtos;
        }
    }
}
