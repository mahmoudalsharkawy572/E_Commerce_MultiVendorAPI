using ECommerce.Domain.Models;

namespace ECommerce.Application.Contracts
{
    public interface ITokenService
    {
        public Task<(string Token, string Jti)> GenerateJwtToken(ApplicationUser user);
        public Task<string> GenerateRefreshToken();
        Task SaveRefreshTokenAsync(string userId, string jwtTokenId, string refreshToken, DateTime expiresAt);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
        Task<(bool IsValid, string? UserId, string? TokenFamilyId, bool TokenReused)> ValidateRefreshTokenAsync(string refreshToken);
    }
}
