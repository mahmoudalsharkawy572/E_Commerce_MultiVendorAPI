using ECommerce.Domain.Models;

namespace ECommerce.Application.Contracts
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetByIdAsync(int id);
        Task<ProductVariant?> GetBySkuAsync(string sku);
        Task AddAsync(ProductVariant productVariant);
        Task<IEnumerable<ProductVariant>> GetByProductIdAsync(int productId);
        Task UpdateAsync(ProductVariant productVariant);
        Task DeleteAsync(int id);
    }
}
