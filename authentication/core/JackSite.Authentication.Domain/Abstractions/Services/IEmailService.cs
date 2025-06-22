using JackSite.Authentication.Const;
using JackSite.Authentication.Enums.Emails;
using JackSite.Domain.Enums;

namespace JackSite.Authentication.Interfaces.Services;

public interface IEmailService
{
    /// <summary>
    /// 发送邮件（基础）
    /// </summary>
    /// <param name="to">收件人邮箱</param>
    /// <param name="subject">主题</param>
    /// <param name="body">正文</param>
    /// <param name="type">邮件类型</param>
    /// <param name="isBodyHtml">正文是否为HTML</param>
    Task SendEmailAsync(string to, string subject, string body,SendEmailType type, bool isBodyHtml = true);

    /// <summary>
    /// 发送邮件（带抄送、密送、附件）
    /// </summary>
    /// <param name="to">收件人邮箱</param>
    /// <param name="subject">主题</param>
    /// <param name="body">正文</param>
    /// <param name="type">邮件类型</param>
    /// <param name="cc">抄送</param>
    /// <param name="bcc">密送</param>
    /// <param name="attachments">附件路径</param>
    /// <param name="isBodyHtml">正文是否为HTML</param>
    Task SendEmailAsync(string to, string subject, string body,SendEmailType type, IEnumerable<string>? cc = null, IEnumerable<string>? bcc = null, IEnumerable<string>? attachments = null, bool isBodyHtml = true);

    /// <summary>
    /// 验证邮箱服务器连接
    /// </summary>
    Task<bool> TestConnectionAsync();

    /// <summary>
    /// 查询邮件发送状态（可选实现，需配合具体实现类）
    /// </summary>
    /// <param name="messageId">邮件唯一标识</param>
    Task<string?> GetSendStatusAsync(string messageId);

    /// <summary>
    /// 获取邮箱配置信息
    /// </summary>
    EmailConfigInfo GetConfigInfo();

    /// <summary>
    /// 批量发送邮件
    /// </summary>
    Task SendBulkEmailAsync(IEnumerable<string> toList, string subject, string body,SendEmailType type ,bool isBodyHtml = true);

    /// <summary>
    /// 预览邮件内容（返回渲染后的HTML）
    /// </summary>
    string PreviewEmailAsync(string subject, string body, bool isBodyHtml = true);

    /// <summary>
    /// 保存草稿
    /// </summary>
    Task SaveDraftAsync(string to, string subject, string body,SendEmailType type, IEnumerable<string>? cc = null, IEnumerable<string>? bcc = null, IEnumerable<string>? attachments = null, bool isBodyHtml = true);

    /// <summary>
    /// 验证邮箱验证码
    /// </summary>
    /// <param name="email"></param>
    /// <param name="code"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<bool> VerifyEmailAsync(string email, string code,SendEmailType type);

    /// <summary>
    /// 根据模板发送邮件
    /// </summary>
    /// <param name="to">收件人邮箱</param>
    /// <param name="templateName">模板名称，不包含扩展名</param>
    /// <param name="type">邮件类型</param>
    /// <param name="parameters">模板参数字典</param>
    /// <param name="subject">邮件主题，如为空则从模板中提取</param>
    /// <returns>发送结果</returns>
    Task SendEmailWithTemplateAsync(
        string to,
        string templateName,
        SendEmailType type,
        Dictionary<string, string> parameters,
        string? subject = null
        );
}

