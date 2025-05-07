using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JackSite.Identity.Server.Data;
using JackSite.Identity.Server.Interfaces;
using JackSite.Identity.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JackSite.Identity.Server.Services
{
   
    public class TokenService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ApplicationDbContext context,
        ILogger<TokenService> logger) : ITokenService
    {
        public async Task<(string accessToken, string refreshToken)> GenerateTokensAsync(ApplicationUser user)
        {
            var accessToken = await GenerateAccessTokenAsync(user);
            var refreshToken = GenerateRefreshToken();
            
            // 保存刷新令牌到数据库
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            
            await context.RefreshTokens.AddAsync(refreshTokenEntity);
            await context.SaveChangesAsync();
            
            return (accessToken, refreshToken);
        }
        
        public async Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken && !t.IsRevoked);
                
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid or expired refresh token");
            }
            
            var user = await userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user == null)
            {
                throw new SecurityTokenException("User not found");
            }
            
            // 撤销旧的刷新令牌
            storedToken.IsRevoked = true;
            context.RefreshTokens.Update(storedToken);
            
            // 生成新的令牌
            var accessToken = await GenerateAccessTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();
            
            // 保存新的刷新令牌
            var refreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            
            await context.RefreshTokens.AddAsync(refreshTokenEntity);
            await context.SaveChangesAsync();
            
            return (accessToken, newRefreshToken);
        }
        
        public async Task RevokeTokenAsync(string refreshToken)
        {
            var storedToken = await context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);
                
            if (storedToken != null)
            {
                storedToken.IsRevoked = true;
                context.RefreshTokens.Update(storedToken);
                await context.SaveChangesAsync();
            }
        }
        
        public async Task<string> GenerateMfaTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("token_type", "mfa")
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5), // MFA 令牌有效期短
                signingCredentials: creds);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public string ValidateMfaToken(string mfaToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };
            
            var principal = tokenHandler.ValidateToken(mfaToken, validationParameters, out var validatedToken);
            
            // 验证令牌类型
            var tokenTypeClaim = principal.FindFirst("token_type");
            if (tokenTypeClaim == null || tokenTypeClaim.Value != "mfa")
            {
                throw new SecurityTokenException("Invalid token type");
            }
            
            // 返回用户 ID
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new SecurityTokenException("User ID not found in token");
            }
            
            return userIdClaim.Value;
        }
        
        private async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);
                
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}