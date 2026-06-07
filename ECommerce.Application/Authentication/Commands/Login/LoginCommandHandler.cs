using ECommerce.Application.Authentication.Dtos;
using ECommerce.Application.Contracts;
using ECommerce.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler(UserManager<ApplicationUser> _userManager,
        ITokenService _tokenService) : IRequestHandler<LoginCommand, TokenDto>
    {
        public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new Exception("invalid credentials");


            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var (accessToken,jti) = await _tokenService.GenerateJwtToken(user);
                var refreshToken = await _tokenService.GenerateRefreshToken();
                var refreshTokenExpiary = DateTime.UtcNow.AddMinutes(5);

                await _tokenService.SaveRefreshTokenAsync(user.Id, jti, refreshToken, refreshTokenExpiary);
                TokenDto tokenDto = new TokenDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(2)
                };
                return tokenDto;
            }

            throw new Exception("invalid credentials");
        }
    }
}
