using ECommerce.Domain.IRepositories;
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
    public class MerchantRepository(ApplicationDbContext _dbContext) : IMerchantRepository
    {
        public async Task<int> CreateAsync(Merchant merchant)
        {
            await _dbContext.Merchants.AddAsync(merchant);
            await _dbContext.SaveChangesAsync();
            return merchant.Id;
        }

        public async Task<Merchant> GetByUserIdAsync(string id)
        {
            var merchant = await _dbContext.Merchants.FirstOrDefaultAsync(m => m.UserId == id);
            return merchant;
        }
    }
}
