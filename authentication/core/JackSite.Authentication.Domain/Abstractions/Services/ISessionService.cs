using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Abstractions.Services;

public interface ISessionService
{
    Task<UserSession> CreateSessionAsync(string userId, string ipAddress, string userAgent);
    Task<bool> ValidateSessionAsync(string sessionId);
    Task<bool> UpdateLastAccessAsync(string sessionId);
    Task<bool> InvalidateSessionAsync(string sessionId);
    Task<IEnumerable<UserSession>> GetActiveSessionsAsync(string userId);
}