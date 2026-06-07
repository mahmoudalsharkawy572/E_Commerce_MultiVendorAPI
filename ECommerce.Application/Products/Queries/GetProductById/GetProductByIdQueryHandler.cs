using AutoMapper;
using ECommerce.Application.Products.Dtos;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler(IMapper _mapper, IUserContext _userContext,
                                            IProductRepository _productRepository ) : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if(user is null)
                throw new Exception("User Not found");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == request.Id && p.Merchant.UserId == user.Id);
            if (product == null)
                throw new Exception($"No Product with Id : {request.Id} ");

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }
    }
}
