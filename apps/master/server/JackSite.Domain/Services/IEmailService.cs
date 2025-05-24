namespace JackSite.Domain.Services;

public interface IEmailService
{
    /// <summary>
    /// 异步发送基础文本邮件。
    /// </summary>
    /// <param name="to">收件人邮箱地址。</param>
    /// <param name="message">邮件正文内容。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task SendEmailAsync(string to, string message,SendEmailType type);

    /// <summary>
    /// 异步发送带主题的邮件。
    /// </summary>
    /// <param name="to">收件人邮箱地址。</param>
    /// <param name="subject">邮件主题。</param>
    /// <param name="message">邮件正文内容。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task SendEmailWithSubjectAsync(string to, string subject, string message,SendEmailType type);

    /// <summary>
    /// 异步批量发送相同内容的邮件。
    /// </summary>
    /// <param name="toList">收件人邮箱地址列表。</param>
    /// <param name="message">邮件正文内容。</param>
    /// <param name="type"></param>
    /// <returns>表示异步操作的任务。</returns>
    Task SendBulkEmailsAsync(List<string> toList, string message,SendEmailType type);

    /// <summary>
    /// 异步发送 HTML 格式的邮件。
    /// </summary>
    /// <param name="to">收件人邮箱地址。</param>
    /// <param name="subject">邮件主题。</param>
    /// <param name="htmlContent">HTML 格式的邮件正文。</param>
    /// <param name="type"></param>
    /// <returns>表示异步操作的任务。</returns>
    Task SendHtmlEmailAsync(string to, string htmlContent, SendEmailType type);
    
    /// <summary>
    /// 异步获取指定邮件的发送状态。
    /// </summary>
    /// <param name="emailId">邮件的唯一标识符。</param>
    /// <returns>表示邮件状态的字符串任务。</returns>
    Task<string> GetEmailStatusAsync(Guid emailId);

    /// <summary>
    /// 异步发送邮件并设置优先级。
    /// </summary>
    /// <param name="to">收件人邮箱地址。</param>
    /// <param name="message">邮件正文内容。</param>
    /// <param name="priority">邮件优先级（低、中、高）。</param>
    /// <returns>表示异步操作的任务。</returns>
    Task SetEmailPriorityAsync(string to, string message, EmailPriorityType priority);
}