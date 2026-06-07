using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Command.UpdateProduct
{
    public class UpdateProductCommandHandler(IProductRepository _productRepository,
                                             IUserContext _userContext) : IRequestHandler<UpdateProductCommand>
    {
        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser is null)
                throw new UnauthorizedAccessException("User not authenticated");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == request.Id);

            if (product is null)
                throw new KeyNotFoundException("Product not found");

            if (product.Merchant.UserId != currentUser.Id)
                throw new UnauthorizedAccessException("You cannot edit this product");

            product.Name = request.Name;
            product.Description = request.Description;
            product.BasePrice = request.BasePrice;

            await _productRepository.UpdateAsync(product);
        }
    }
}
