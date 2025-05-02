using JackSite.Notification.Grpc.Interfaces;
using JackSite.Notification.Grpc.Services;
using JackSite.Shared.EventBus.Abstractions;
using JackSite.Shared.EventBus.Events;

namespace JackSite.Notification.Grpc.EventHandlers;

/// <summary>
/// 用户注册事件处理器
/// </summary>
public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<UserRegisteredEventHandler> _logger;
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public UserRegisteredEventHandler(
        IEmailService emailService,
        ILogger<UserRegisteredEventHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }
    
    /// <summary>
    /// 处理事件
    /// </summary>
    public async Task HandleAsync(UserRegisteredEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("处理用户注册事件，用户: {@event.Username}, 邮箱: {@event.Email}", @event.Username, @event.Email);
        
        try
        {
            // 发送验证邮件
            await _emailService.SendRegistrationVerificationEmailAsync(
                @event.Email,
                @event.Username,
                @event.VerificationToken);
                
            _logger.LogInformation("已成功发送验证邮件到 {@event.Email}", @event.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送验证邮件失败: {Message}", ex.Message);
        }
    }
}