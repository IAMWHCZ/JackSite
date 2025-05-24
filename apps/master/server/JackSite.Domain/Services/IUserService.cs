using JackSite.Domain.Entities;

namespace JackSite.Domain.Services;

public interface IUserService
{
    /// <summary>
    /// 验证用户凭据并返回用户信息
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>如果凭据有效，则返回用户信息；否则返回null</returns>
    /// <remarks>
    /// 此方法验证提供的用户名和密码是否匹配系统中的有效用户。
    /// 如果验证成功，返回用户的基本信息；如果验证失败，返回null。
    /// </remarks>
    Task<UserBasic?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// 注册新用户
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="email">电子邮件地址</param>
    /// <param name="password">密码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>新创建的用户信息</returns>
    /// <remarks>
    /// 此方法创建一个新用户账户，包括用户名、电子邮件和密码。
    /// 如果用户名或电子邮件已存在，将抛出异常。
    /// </remarks>
    /// <exception cref="InvalidOperationException">当用户名或电子邮件已被使用时抛出</exception>
    Task<UserBasic> RegisterAsync(string username, string email, string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 发送验证邮件
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <param name="type">邮件类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>表示异步操作的任务</returns>
    /// <remarks>
    /// 此方法根据指定的类型向用户发送验证邮件，
    /// 例如注册确认、密码重置或电子邮件更改等。
    /// </remarks>
    Task SendVerificationEmailAsync(UserBasic user, SendEmailType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// 验证邮件验证令牌
    /// </summary>
    /// <param name="verificationToken">验证令牌</param>
    /// <param name="type">邮件类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>如果验证成功，则返回true；否则返回false</returns>
    /// <remarks>
    /// 此方法验证用户提供的邮件验证令牌是否有效。
    /// 验证令牌通常通过邮件发送给用户，用于确认特定操作，
    /// 如账户注册、密码重置或电子邮件更改等。
    /// </remarks>
    Task<bool> VerifyEmailAsync(string email,string code, SendEmailType type,
        CancellationToken cancellationToken = default);
}