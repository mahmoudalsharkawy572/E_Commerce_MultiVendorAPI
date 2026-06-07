using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Products.Command.DeleteProduct
{
    public class DeleteProductCommandHandler(IUserContext _userContext,IProductRepository _productRepository) : IRequestHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser is null)
                throw new UnauthorizedAccessException("User is not authenticated");

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == request.Id);

            if (product is null)
                throw new KeyNotFoundException("Product not found");

           
            if (product.Merchant.UserId != currentUser.Id)
                throw new UnauthorizedAccessException("You are not allowed to delete this product");

            await _productRepository.DeleteAsync(product);
        }
    }
}
