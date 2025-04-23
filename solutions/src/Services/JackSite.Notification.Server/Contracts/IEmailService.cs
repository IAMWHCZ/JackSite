using System.Collections.Concurrent;

namespace JackSite.Notification.Server.Contracts;

public interface IEmailService
{
    /// <summary>
    /// 发送简单邮件
    /// </summary>
    /// <param name="to">收件人邮箱地址</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    Task SendEmailAsync(string to, string subject, string body);

    /// <summary>
    /// 发送带单个附件的邮件
    /// </summary>
    /// <param name="to">收件人邮箱地址</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    /// <param name="attachmentPath">附件文件路径</param>
    Task SendEmailAsync(string to, string subject, string body, string attachmentPath);

    /// <summary>
    /// 发送带多个附件的邮件
    /// </summary>
    /// <param name="to">收件人邮箱地址</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    /// <param name="attachmentPaths">附件文件路径集合</param>
    Task SendEmailAsync(string to, string subject, string body, IEnumerable<string> attachmentPaths);

    /// <summary>
    /// 发送邮件给多个收件人
    /// </summary>
    /// <param name="to">收件人邮箱地址集合</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="body">邮件内容（支持HTML）</param>
    Task SendEmailAsync(IEnumerable<string> to, string subject, string body);

    /// <summary>
    /// 使用模板发送邮件
    /// </summary>
    /// <param name="to">收件人邮箱地址</param>
    /// <param name="subject">邮件主题</param>
    /// <param name="templateName">模板名称</param>
    /// <param name="model">用于渲染模板的数据模型</param>
    Task SendEmailWithTemplateAsync(string to, string subject, string templateName, object model);

    /// <summary>
    /// 批量发送邮件
    /// </summary>
    /// <param name="emails">邮件信息集合，每项包含收件人地址、主题和内容</param>
    Task SendBulkEmailAsync(IEnumerable<(string Email, string Subject, string Body)> emails);

    /// <summary>
    /// 批量处理邮件发送，支持并发控制和错误收集
    /// </summary>
    /// <param name="batch">要处理的邮件批次，每项包含收件人、主题、内容和可选的附件</param>
    /// <param name="semaphore">用于控制并发发送数量的信号量</param>
    /// <param name="failedEmails">用于收集发送失败的邮件信息的线程安全容器</param>
    Task ProcessEmailBatchAsync(
        List<(string Email, string Subject, string Body, IEnumerable<string>? Attachments)> batch,
        SemaphoreSlim semaphore,
        ConcurrentBag<(string Email, Exception Exception)> failedEmails);

    /// <summary>
    /// 删除指定ID的邮件记录
    /// </summary>
    /// <param name="id">要删除的邮件ID</param>
    Task DeleteEmailAsync(SnowflakeId id);
}