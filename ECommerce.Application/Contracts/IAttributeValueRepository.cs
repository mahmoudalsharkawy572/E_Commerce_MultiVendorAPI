using ECommerce.Domain.Models;

namespace ECommerce.Application.Contracts
{
    public interface IAttributeValueRepository
    {
        Task<AttributeValue?> GetByIdAsync(int id);
        Task AddAsync(AttributeValue attributeValue);
        Task<IEnumerable<AttributeValue>> GetByProductAttributeIdAsync(int productAttributeId);
        Task<bool> ExistsAsync(int productAttributeId, string value);
  
    }
}
