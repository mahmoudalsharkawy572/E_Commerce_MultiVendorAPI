using AutoMapper;
using ECommerce.Application.Contracts;
using ECommerce.Application.ProductAttributes.Dtos;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.ProductAttributes.Queries.GetAttributesForProductById
{
    public class GetAttributesForProductByIdQueryHandler(IMapper _mapper, IProductAttributeRepository _productAttributeRepository,
                                                         IUserContext _userContext, IProductRepository _productRepository) : IRequestHandler<GetAttributesForProductByIdQuery, IEnumerable<ProductAttributeDto>>
    {
        public async Task<IEnumerable<ProductAttributeDto>> Handle(GetAttributesForProductByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (user == null)
                throw new Exception("User not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == request.ProductId);

            if (product == null)
                throw new Exception($"Product with ID {request.ProductId} not found");

            if (product.Merchant.UserId != user.Id)
                throw new Exception("Unauthorized: You are not the owner of this product");

            var productAttributes = await _productAttributeRepository.GetByProductIdAsync(request.ProductId);

            var productAttributeDtos = _mapper.Map<IEnumerable<ProductAttributeDto>>(productAttributes);
            return productAttributeDtos;
        }
    }
}
