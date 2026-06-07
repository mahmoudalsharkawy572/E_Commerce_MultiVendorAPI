using ECommerce.Application.Contracts;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductAttributeRepository(ApplicationDbContext _dbContext) : IProductAttributeRepository
    {
        public async Task<ProductAttribute?> GetByIdAsync(int id)
        {
            return await _dbContext.ProductAttributes.Include(x => x.Values).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int productId, string attributeName)
        {
            return await _dbContext.ProductAttributes.AnyAsync(x => x.ProductId == productId && x.Name == attributeName);
        }

        public async Task AddAsync(ProductAttribute productAttribute)
        {
            await _dbContext.ProductAttributes.AddAsync(productAttribute);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductAttribute>> GetByProductIdAsync(int productId)
        {
            return await _dbContext.ProductAttributes.Include(x => x.Values).Where(x => x.ProductId == productId).ToListAsync();
        }
    }
}
