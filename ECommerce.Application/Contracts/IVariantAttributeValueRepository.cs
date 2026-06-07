using ECommerce.Domain.Models;

namespace ECommerce.Application.Contracts
{
    public interface IVariantAttributeValueRepository
    {
        Task<VariantAttributeValue?> GetByIdAsync(int id);
        Task AddAsync(VariantAttributeValue variantAttributeValue);
        Task<IEnumerable<VariantAttributeValue>> GetByProductVariantIdAsync(int productVariantId);
        Task<bool> ExistsAsync(int productVariantId, int attributeValueId);
        Task DeleteAsync(int id);
    }
}
