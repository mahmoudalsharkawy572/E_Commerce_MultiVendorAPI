using AutoMapper;
using ECommerce.Domain.Models;

namespace ECommerce.Application.Authentication.Dtos
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<ApplicationUser, AppUserDto>();
        }
    }
}
