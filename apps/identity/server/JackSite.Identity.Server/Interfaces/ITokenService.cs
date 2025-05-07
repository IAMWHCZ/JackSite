using JackSite.Identity.Server.Models;

namespace JackSite.Identity.Server.Interfaces
{
     public interface ITokenService
    {
        Task<(string accessToken, string refreshToken)> GenerateTokensAsync(ApplicationUser user);
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(string refreshToken);
        Task<string> GenerateMfaTokenAsync(ApplicationUser user);
        string ValidateMfaToken(string mfaToken);
    }
    
}