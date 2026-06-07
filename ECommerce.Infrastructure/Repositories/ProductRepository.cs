using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository(ApplicationDbContext _dbContext) : IProductRepository
    {
        public async Task<int> CreateAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product.Id;
        }

        public async Task DeleteAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(Expression<Func<Product, bool>> expression)
        {
            IEnumerable<Product> products = await _dbContext.Products.Where(expression).ToListAsync();
            return products;
        }
        public async Task<(IEnumerable<Product>, int)> GetAllProductsAsync(Expression<Func<Product, bool>> expression
            , string? searchPhrase,
            int pageSize,
            int pageNumber)
        {
            if(pageNumber <= 0) pageNumber = 1; 
            if(pageSize <= 0 || pageSize>=10) pageSize = 10;

            var searchPhraseLower = searchPhrase?.ToLower();

            var baseQuery = _dbContext
                .Products
                .Where(expression)
                .Where(r => searchPhraseLower == null || (r.Name.ToLower().Contains(searchPhraseLower)
                                                       || r.Description.ToLower().Contains(searchPhraseLower)));

            var totalCount = await baseQuery.CountAsync();

            var products = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<Product> GetProductByIdAsync(Expression<Func<Product, bool>> expression)
        {
            var product = await _dbContext.Products.Include(p => p.Merchant).Where(expression).FirstOrDefaultAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
