using ECommerce.Application.Authentication.Dtos;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Register
{
    public class RegisterCommand : IRequest<AppUserDto>
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
