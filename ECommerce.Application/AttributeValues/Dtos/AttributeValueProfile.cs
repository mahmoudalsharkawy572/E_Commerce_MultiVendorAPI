using AutoMapper;
using ECommerce.Domain.Models;

namespace ECommerce.Application.AttributeValues.Dtos
{
    public class AttributeValueProfile : Profile
    {
        public AttributeValueProfile()
        {
            CreateMap<AttributeValue, AttributeValueDto>();
            CreateMap<AttributeValueDto, AttributeValue>();
        }
    }
}
