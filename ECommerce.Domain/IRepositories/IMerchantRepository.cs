using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.IRepositories
{
    public interface IMerchantRepository
    {
        public Task<Merchant> GetByUserIdAsync(string id);
        public Task<int> CreateAsync(Merchant merchant);
    }
}
