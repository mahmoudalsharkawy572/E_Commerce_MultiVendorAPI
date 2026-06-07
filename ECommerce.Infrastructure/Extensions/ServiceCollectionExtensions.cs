using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ECommerceDB");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
            services.AddScoped<IAttributeValueRepository, AttributeValueRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IVariantAttributeValueRepository, VariantAttributeValueRepository>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<ITokenService, TokenService>();
            
            



        }
    }
}
