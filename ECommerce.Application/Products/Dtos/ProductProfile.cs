using AutoMapper;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Products.Dtos
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
