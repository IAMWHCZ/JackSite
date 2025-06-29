using System.Security.Claims;
using JackSite.Authentication.Entities.Clients;
using JackSite.Authentication.Entities.Users;

namespace JackSite.Authentication.Abstractions.Services;

public interface ITokenService
{
    /// <summary>
    /// 生成访问令牌
    /// </summary>
    /// <param name="user">用户基础信息</param>
    /// <param name="session">用户会话信息</param>
    /// <param name="client">客户端</param>
    /// <returns>JWT 访问令牌字符串</returns>
    Task<string> GenerateAccessTokenAsync(
        UserBasic user, 
        UserSession session,
        ClientBasic client
            );

    /// <summary>
    /// 生成刷新令牌
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>刷新令牌字符串</returns>
    Task<string> GenerateRefreshToken(UserBasic user,ClientBasic client);

    /// <summary>
    /// 验证并解析令牌
    /// </summary>
    /// <param name="token">JWT 令牌字符串</param>
    /// <returns>包含用户声明信息的 ClaimsPrincipal 对象，验证失败返回 null</returns>
    ClaimsPrincipal ValidateToken(string token);

    /// <summary>
    /// 使用刷新令牌获取新的访问令牌
    /// </summary>
    /// <param name="refreshToken">刷新令牌字符串</param>
    /// <returns>新的访问令牌字符串</returns>
    Task<string> RefreshTokenAsync(string refreshToken);
}