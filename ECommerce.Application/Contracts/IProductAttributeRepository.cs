using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IProductAttributeRepository
    {
        Task<ProductAttribute?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int productId,string attributeName);
        Task AddAsync(ProductAttribute productAttribute);
        Task<IEnumerable<ProductAttribute>> GetByProductIdAsync(int productId);

    }
}
