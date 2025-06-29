using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Entities.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace JackSite.Authentication.Infrastructure.Services;

public class TokenService(
    IConfiguration configuration,
    ISessionService sessionService,
    IAccessBaseService accessBaseService
) : ITokenService
{
    public async Task<string> GenerateAccessTokenAsync(UserBasic user, UserSession session, ClientBasic client)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(client.Secret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.Sha256);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Gender, user.UserProfile?.Gender ?? "男"),
            new("ClientId", client.Id.ToString()),
            new("SessionId", session.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
        };
        var token = new JwtSecurityToken(
            issuer: "JackSite",
            audience: client.Name,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(client.AccessTokenLifespan),
            signingCredentials: signingCredentials
        );
        var formBase = accessBaseService.GetCurrentFormBase();
        await sessionService.CreateSessionAsync(user.Id.ToString(), formBase.IPAdress, formBase.UserAgent);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Task<string> GenerateRefreshToken(UserBasic user,ClientBasic client)
    {
        
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        throw new NotImplementedException();
    }

    public Task<string> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}