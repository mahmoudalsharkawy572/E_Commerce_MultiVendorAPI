using AutoMapper;
using ECommerce.Domain.Models;

namespace ECommerce.Application.ProductVariants.Dtos
{
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<ProductVariantDto, ProductVariant>();
        }
    }
}
