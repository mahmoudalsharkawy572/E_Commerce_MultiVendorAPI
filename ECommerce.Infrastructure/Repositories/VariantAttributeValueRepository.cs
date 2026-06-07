using ECommerce.Application.Contracts;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class VariantAttributeValueRepository(ApplicationDbContext _dbContext) : IVariantAttributeValueRepository
    {
        public async Task<VariantAttributeValue?> GetByIdAsync(int id)
        {
            return await _dbContext.VariantAttributeValues.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(VariantAttributeValue variantAttributeValue)
        {
            await _dbContext.VariantAttributeValues.AddAsync(variantAttributeValue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<VariantAttributeValue>> GetByProductVariantIdAsync(int productVariantId)
        {
            return await _dbContext.VariantAttributeValues
                .Where(x => x.ProductVariantId == productVariantId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int productVariantId, int attributeValueId)
        {
            return await _dbContext.VariantAttributeValues
                .AnyAsync(x => x.ProductVariantId == productVariantId && x.AttributeValueId == attributeValueId);
        }

        public async Task DeleteAsync(int id)
        {
            var variantAttributeValue = await _dbContext.VariantAttributeValues.FirstOrDefaultAsync(x => x.Id == id);
            if (variantAttributeValue != null)
            {
                _dbContext.VariantAttributeValues.Remove(variantAttributeValue);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
