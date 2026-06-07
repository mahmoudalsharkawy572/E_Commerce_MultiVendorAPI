using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using MediatR;

namespace ECommerce.Application.ProductAttributes.Commands.CreateProductAttribute
{

    public class CreateProductAttributeCommandHandler(IUserContext _userContext, IProductAttributeRepository _productAttributeRepository,
       IProductRepository _productRepository ) : IRequestHandler<CreateProductAttributeCommand, int>
    {
        public async Task<int> Handle(CreateProductAttributeCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.GetCurrentUser()!.Id;

            var product = await _productRepository.GetProductByIdAsync(p => p.Id == request.ProductId);

            if (product is null)
                throw new KeyNotFoundException("Product not found.");

            if (product.Merchant.UserId != currentUserId)
                throw new UnauthorizedAccessException(
                    "You cannot manage another merchant's products.");

            var attributeExists = await _productAttributeRepository.ExistsAsync(product.Id,request.Name);

            if (attributeExists)
                throw new InvalidOperationException(
                    $"Attribute '{request.Name}' already exists.");

            var attribute = new ProductAttribute
            {
                ProductId = request.ProductId,
                Name = request.Name
            };

            await _productAttributeRepository.AddAsync(attribute);

            return attribute.Id;
        }
    }
}
