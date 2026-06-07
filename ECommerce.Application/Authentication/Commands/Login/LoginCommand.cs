using ECommerce.Application.Authentication.Dtos;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Login
{
    public class LoginCommand : IRequest<TokenDto>
    {
        public string Email { get; set; }
        public string Password { get; set; } 
    }
}
