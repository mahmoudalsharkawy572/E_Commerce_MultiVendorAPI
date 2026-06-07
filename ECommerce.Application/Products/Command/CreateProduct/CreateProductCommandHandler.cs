using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler(IUserContext _userContext,
        IProductRepository _productRepository, IMerchantRepository _merchantRepository) : IRequestHandler<CreateProductCommand, int>
    {
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if(user == null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var merchant = await _merchantRepository.GetByUserIdAsync(user.Id);
            if (merchant == null)
                throw new InvalidOperationException("User is not a merchant.");

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                BasePrice = request.BasePrice,
                MerchantId = merchant.Id
            };

            var productId = await _productRepository.CreateAsync(product);

            return productId;
        }
    }
}
