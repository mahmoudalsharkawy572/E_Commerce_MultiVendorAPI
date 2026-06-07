using ECommerce.Application.Authentication.Dtos;
using ECommerce.Application.Contracts;
using ECommerce.Application.Users;
using ECommerce.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerce.Application.Authentication.Commands.RefreshAccessToken
{
    public class RefreshAccessTokenCommandHandler(ITokenService _tokenService,UserManager<ApplicationUser> _userManager) 
                                                                  : IRequestHandler<RefreshAccessTokenCommand, TokenDto>
    {
        public async Task<TokenDto> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                throw new UnauthorizedAccessException("Refresh token is required");

            var (isValid, userId, tokenFamilyId, tokenReused) =
                await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);

            if (tokenReused)
                throw new SecurityTokenException("Refresh token reuse detected");

            if (!isValid || string.IsNullOrEmpty(userId))
                throw new SecurityTokenException("Invalid refresh token");

            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
                throw new SecurityTokenException("User not found");

            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

            var (accessToken, jti) = await _tokenService.GenerateJwtToken(appUser);

            var newRefreshToken = await _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddMinutes(5);

            await _tokenService.SaveRefreshTokenAsync(
                appUser.Id,
                jti,
                newRefreshToken,
                refreshTokenExpiry);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(2)
            };
        }
    }
}

