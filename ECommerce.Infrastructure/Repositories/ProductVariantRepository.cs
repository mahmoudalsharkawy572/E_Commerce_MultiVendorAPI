using ECommerce.Application.Contracts;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductVariantRepository(ApplicationDbContext _dbContext) : IProductVariantRepository
    {
        public async Task<ProductVariant?> GetByIdAsync(int id)
        {
            return await _dbContext.ProductVariants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProductVariant?> GetBySkuAsync(string sku)
        {
            return await _dbContext.ProductVariants.FirstOrDefaultAsync(x => x.SKU == sku);
        }

        public async Task AddAsync(ProductVariant productVariant)
        {
            await _dbContext.ProductVariants.AddAsync(productVariant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(int productId)
        {
            return await _dbContext.ProductVariants
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task UpdateAsync(ProductVariant productVariant)
        {
            _dbContext.ProductVariants.Update(productVariant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var productVariant = await _dbContext.ProductVariants.FirstOrDefaultAsync(x => x.Id == id);
            if (productVariant != null)
            {
                _dbContext.ProductVariants.Remove(productVariant);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
