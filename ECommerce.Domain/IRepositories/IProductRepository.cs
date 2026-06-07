using ECommerce.Domain.Models;
using System.Linq.Expressions;

namespace ECommerce.Domain.IRepositories
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllProductsAsync(Expression<Func<Product, bool>> expression);
        public Task<Product> GetProductByIdAsync(Expression<Func<Product, bool>> expression);
        public Task<(IEnumerable<Product>, int)> GetAllProductsAsync(Expression<Func<Product, bool>> expression
            , string? searchPhrase,
            int pageSize,
            int pageNumber);
        public Task<int> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        public Task DeleteAsync(Product product);

    }
}
