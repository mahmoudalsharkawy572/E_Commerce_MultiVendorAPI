using ECommerce.Application.Contracts;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class TokenService(ApplicationDbContext _dbContext
                          , IConfiguration _configuration)  : ITokenService
    {
        public async Task<(string Token, string Jti)> GenerateJwtToken(ApplicationUser user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["Key"]!);
            var jti = Guid.NewGuid().ToString();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,jti)
                }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token),jti);
        }

        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            var exists = await _dbContext.RefreshTokens.AnyAsync(rt => rt.RefreshTokenValue == refreshToken);
            if (exists)
            {
                return await GenerateRefreshToken();
            }
            return refreshToken;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshTokenValue == refreshToken);
            if (storedToken == null)
                return false;
            else
            {
                storedToken.IsValid = false;
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task SaveRefreshTokenAsync(string userId, string jwtTokenId, string refreshToken, DateTime expiresAt)
        {
            var refreshTokenEntity = new RefreshToken
            {
                UserId = userId,
                JwtTokenId = jwtTokenId,
                RefreshTokenValue = refreshToken,
                ExpiresAt = expiresAt,
                IsValid = true
            };
            await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(bool IsValid, string? UserId, string? TokenFamilyId, bool TokenReused)> ValidateRefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.RefreshTokenValue == refreshToken);
            if (storedToken == null)
                return (false, null, null, false);

            if (!storedToken.IsValid)
            {
                var relatedTokens = await _dbContext.RefreshTokens.Where(rt => rt.JwtTokenId == storedToken.JwtTokenId && rt.UserId == storedToken.UserId).ToListAsync();
                if (relatedTokens.Count > 0)
                {
                    foreach (var token in relatedTokens)
                    {
                        token.IsValid = false;
                    }
                    await _dbContext.SaveChangesAsync();
                }
                return (false, storedToken.UserId, storedToken.JwtTokenId, true);
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
                return (false, storedToken.UserId, storedToken.JwtTokenId, false);

            return (true, storedToken.UserId, storedToken.JwtTokenId, false);
        }
    }
}
