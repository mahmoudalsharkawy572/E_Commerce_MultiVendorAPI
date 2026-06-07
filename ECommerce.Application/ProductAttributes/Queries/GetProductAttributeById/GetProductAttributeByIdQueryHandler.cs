using AutoMapper;
using ECommerce.Application.Contracts;
using ECommerce.Application.ProductAttributes.Dtos;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;

namespace ECommerce.Application.ProductAttributes.Queries.GetProductAttributeById
{
    public class GetProductAttributeByIdQueryHandler(IMapper _mapper, IProductAttributeRepository _productAttributeRepository,
                                                     IUserContext _userContext,IProductRepository _productRepository) : IRequestHandler<GetProductAttributeByIdQuery, ProductAttributeDto>
    {
        public async Task<ProductAttributeDto> Handle(GetProductAttributeByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null) 
                throw new Exception("User not found");

            var productAttribute = await _productAttributeRepository.GetByIdAsync(request.Id);

            if (productAttribute == null)
                throw new Exception($"Product attribute with ID {request.Id} not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == productAttribute.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            var productAttributeDto = _mapper.Map<ProductAttributeDto>(productAttribute);
            return productAttributeDto;
        }
    }
}
