namespace JackSite.Authentication.Abstractions.Services;

public interface IAuthenticationService
{
    Task AuthenticateAsync(string username, string password);
    Task ValidateTokenAsync(string token);
    Task<bool> LogoutAsync(string userId, string sessionId);
    Task<bool> LogoutAllSessionsAsync(string userId);
}