using ECommerce.Application.Contracts;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class AttributeValueRepository(ApplicationDbContext _dbContext) : IAttributeValueRepository
    {
        public async Task<AttributeValue?> GetByIdAsync(int id)
        {
            return await _dbContext.AttributeValues.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(AttributeValue attributeValue)
        {
            await _dbContext.AttributeValues.AddAsync(attributeValue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AttributeValue>> GetByProductAttributeIdAsync(int productAttributeId)
        {
            return await _dbContext.AttributeValues
                .Where(x => x.ProductAttributeId == productAttributeId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int productAttributeId, string value)
        {
            return await _dbContext.AttributeValues
                .AnyAsync(x => x.ProductAttributeId == productAttributeId && x.Value == value);
        }

    
    }
}
