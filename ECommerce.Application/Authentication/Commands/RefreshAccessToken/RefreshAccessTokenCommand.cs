using ECommerce.Application.Authentication.Dtos;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.RefreshAccessToken
{
    public class RefreshAccessTokenCommand  : IRequest<TokenDto>
    {
        public string RefreshToken { get; set; } = default!;
    }
}
