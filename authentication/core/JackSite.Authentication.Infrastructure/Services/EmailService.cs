using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using JackSite.Authentication.Abstractions.Repositories;
using JackSite.Authentication.Abstractions.Services;
using JackSite.Authentication.Entities.Emails;
using JackSite.Authentication.Enums.Emails;
using JackSite.Authentication.Exceptions;
using JackSite.Authentication.Interfaces.Services;
using JackSite.Authentication.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace JackSite.Authentication.Infrastructure.Services;

public class EmailService(
    ICacheService cacheService,
    IRepository<EmailBasic> emailRepository,
    IConfiguration configuration
) : IEmailService
{
    public async Task SendEmailAsync(
        string to,
        string subject,
        string body,
        SendEmailType type,
        bool isBodyHtml = true
    )
    {
        try
        {
            using var smtpClient = GetSmtpClient();
            var email = new EmailBasic
            {
                Title = subject,
                Type = type,
            };
            email.EmailContent = new EmailContent
            {
                Content = body,
                Subject = subject,
                EmailId = email.Id
            };

            await emailRepository.AddAsync(email);
            await emailRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new EmailException(e.Message, to, e);
        }
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string body,
        SendEmailType type,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null,
        IEnumerable<string>? attachments = null,
        bool isBodyHtml = true
    )
    {
        try
        {
            using var smtpClient = GetSmtpClient();
            var config = GetConfigInfo();

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config.UserName, config.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            // 添加收件人
            mailMessage.To.Add(to);

            // 添加抄送人
            var enumerable = cc as string[] ?? cc.ToArray();
            if (enumerable.Length > 0)
            {
                foreach (var ccAddress in enumerable.ToList().Where(ccAddress => !string.IsNullOrEmpty(ccAddress)))
                {
                    mailMessage.CC.Add(ccAddress);
                }
            }

            // 添加密送人
            if (bcc != null)
            {
                foreach (var bccAddress in bcc)
                {
                    if (!string.IsNullOrEmpty(bccAddress))
                    {
                        mailMessage.Bcc.Add(bccAddress);
                    }
                }
            }

            // 添加附件
            var emailAttachments = new List<EmailAttachment>();
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    if (!string.IsNullOrEmpty(attachment) && File.Exists(attachment))
                    {
                        var fileInfo = new FileInfo(attachment);
                        mailMessage.Attachments.Add(new Attachment(attachment));

                        emailAttachments.Add(new EmailAttachment
                        {
                            ObjectKey = attachment,
                            FileName = fileInfo.Name,
                            FileSize = fileInfo.Length,
                            FileExtension = fileInfo.Extension,
                            ContentType = fileInfo.Extension.ToMimeType(),
                            StorageType = "local",
                            UploadTime = DateTime.UtcNow
                        });
                    }
                }
            }

            // 发送邮件
            await smtpClient.SendMailAsync(mailMessage);

            // 保存邮件记录到数据库
            var email = new EmailBasic
            {
                Title = subject,
                Type = type,
                SenderEmail = config.UserName,
                SenderName = config.DisplayName,
                Status = EmailStatus.Sent,
                SentTime = DateTime.UtcNow,
                MessageId = mailMessage.Headers["Message-ID"]
            };

            email.EmailContent = new EmailContent
            {
                Content = body,
                Subject = subject,
                EmailId = email.Id,
                IsHtml = isBodyHtml,
                Recipient = to,
                CC = cc != null ? string.Join(";", enumerable) : null,
                BCC = bcc != null ? string.Join(";", bcc) : null,
                PlainTextContent = isBodyHtml ? body.StripHtml() : body
            };

            // 添加收件人记录
            email.EmailRecipients.Add(new EmailRecipient
            {
                RecipientEmail = to,
                Type = RecipientType.To,
                Status = DeliveryStatus.Sent,
                EmailRecordId = email.Id
            });

            // 添加抄送人记录
            if (cc != null)
            {
                foreach (var ccAddress in enumerable.Where(a => !string.IsNullOrEmpty(a)))
                {
                    email.EmailRecipients.Add(new EmailRecipient
                    {
                        RecipientEmail = ccAddress,
                        Type = RecipientType.CC,
                        Status = DeliveryStatus.Sent,
                        EmailRecordId = email.Id
                    });
                }
            }

            // 添加密送人记录
            if (bcc != null)
            {
                foreach (var bccAddress in bcc.Where(a => !string.IsNullOrEmpty(a)))
                {
                    email.EmailRecipients.Add(new EmailRecipient
                    {
                        RecipientEmail = bccAddress,
                        Type = RecipientType.BCC,
                        Status = DeliveryStatus.Sent,
                        EmailRecordId = email.Id
                    });
                }
            }

            // 添加附件记录
            if (emailAttachments.Any())
            {
                foreach (var attachment in emailAttachments)
                {
                    attachment.EmailRecordId = email.Id;
                    email.EmailAttachments.Add(attachment);
                }
            }

            await emailRepository.AddAsync(email);
            await emailRepository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new EmailException(e.Message, to, e);
        }
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var config = GetConfigInfo();
            if (config == null)
            {
                throw new EmailException("无法获取邮件配置信息");
            }

            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(config.SmtpServer, config.SmtpPort);
            if (!tcpClient.Connected)
            {
                throw new EmailException($"无法连接到SMTP服务器 {config.SmtpServer}:{config.SmtpPort}");
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new EmailException("邮件服务连接测试失败", ex);
        }
    }

    public async Task<string?> GetSendStatusAsync(string messageId)
    {
        if (string.IsNullOrEmpty(messageId))
        {
            throw new ArgumentNullException(nameof(messageId));
        }

        var email = await emailRepository.FirstOrDefaultAsync(e => e.MessageId == messageId);
        if (email == null)
        {
            return null;
        }

        return email.Status switch
        {
            EmailStatus.Draft => "草稿",
            EmailStatus.Queued => "队列中",
            EmailStatus.Sending => "发送中",
            EmailStatus.Sent => "已发送",
            EmailStatus.Failed => $"发送失败: {email.FailureReason}",
            EmailStatus.Cancelled => "已取消",
            _ => "未知状态"
        };
    }

    public EmailConfigInfo GetConfigInfo()
    {
        var emailConfigInfo = new EmailConfigInfo();
        configuration.GetSection("Email").Bind(emailConfigInfo);

        if (emailConfigInfo.SmtpServer is null)
        {
            emailConfigInfo = new EmailConfigInfo
            {
                SmtpServer = "smtp-mail.outlook.com",
                SmtpPort = 587,
                UserName = "cz2545481217@outlook.com",
                Password = "Cz18972621866!@#",
                DisplayName = "系统通知",
                EnableSsl = true
            };    
        }

        return emailConfigInfo;
    }

    public async Task SendBulkEmailAsync(
        IEnumerable<string> toList,
        string subject,
        string body,
        SendEmailType type,
        bool isBodyHtml = true)
    {
        var enumerable = toList as string[] ?? toList.ToArray();
        if (toList == null || enumerable.Length == 0)
        {
            throw new ArgumentException("收件人列表不能为空", nameof(toList));
        }

        using var smtpClient =  GetSmtpClient();
        var config = GetConfigInfo();
        var success = new List<string>();
        if (success == null)
        {
            var exception = new ArgumentNullException(nameof(success))
            {
                HelpLink = null,
                HResult = 0,
                Source = null
            };
            throw exception;
        }

        var failed = new Dictionary<string, string>();

        foreach (var to in enumerable)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(config.UserName, config.DisplayName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isBodyHtml
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);

                // 记录成功的邮件
                var email = new EmailBasic
                {
                    Title = subject,
                    Type = type,
                    SenderEmail = config.UserName,
                    SenderName = config.DisplayName,
                    Status = EmailStatus.Sent,
                    SentTime = DateTime.UtcNow,
                    MessageId = mailMessage.Headers["Message-ID"]
                };

                email.EmailContent = new EmailContent
                {
                    Content = body,
                    Subject = subject,
                    EmailId = email.Id,
                    IsHtml = isBodyHtml,
                    Recipient = to,
                    PlainTextContent = isBodyHtml ? body.StripHtml() : body
                };

                email.EmailRecipients.Add(new EmailRecipient
                {
                    RecipientEmail = to,
                    Type = RecipientType.To,
                    Status = DeliveryStatus.Sent,
                    EmailRecordId = email.Id
                });

                await emailRepository.AddAsync(email);
                success.Add(to);
            }
            catch (Exception ex)
            {
                // 记录失败的邮件
                var email = new EmailBasic
                {
                    Title = subject,
                    Type = type,
                    SenderEmail = config.UserName,
                    SenderName = config.DisplayName,
                    Status = EmailStatus.Failed,
                    SentTime = DateTime.UtcNow,
                    FailureReason = ex.Message
                };

                email.EmailContent = new EmailContent
                {
                    Content = body,
                    Subject = subject,
                    EmailId = email.Id,
                    IsHtml = isBodyHtml,
                    Recipient = to,
                    PlainTextContent = isBodyHtml ? body.StripHtml() : body
                };

                email.EmailRecipients.Add(new EmailRecipient
                {
                    RecipientEmail = to,
                    Type = RecipientType.To,
                    Status = DeliveryStatus.Failed,
                    FailureReason = ex.Message,
                    EmailRecordId = email.Id
                });

                await emailRepository.AddAsync(email);
                failed.Add(to, ex.Message);
            }
        }

        await emailRepository.SaveChangesAsync();

        // 如果有失败的邮件，抛出异常
        if (failed.Count != 0)
        {
            var failedMessage = string.Join(", ", failed.Select(kv => $"{kv.Key}: {kv.Value}"));
            throw new EmailException($"批量邮件发送部分失败: {failedMessage}");
        }
    }

    public string PreviewEmailAsync(string subject, string body, bool isBodyHtml = true)
    {
        try
        {
            var config = GetConfigInfo();

            // 构建邮件预览HTML
            var preview = new StringBuilder();
            preview.AppendLine("<!DOCTYPE html>");
            preview.AppendLine("<html lang=\"zh-CN\">");
            preview.AppendLine("<head>");
            preview.AppendLine("  <meta charset=\"UTF-8\">");
            preview.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            preview.AppendLine("  <title>邮件预览</title>");
            preview.AppendLine("  <style>");
            preview.AppendLine("    body { font-family: Arial, sans-serif; margin: 0; padding: 20px; color: #333; }");
            preview.AppendLine(
                "    .email-preview { max-width: 650px; margin: 0 auto; border: 1px solid #ddd; border-radius: 5px; overflow: hidden; }");
            preview.AppendLine(
                "    .email-header { background-color: #f5f5f5; padding: 15px; border-bottom: 1px solid #ddd; }");
            preview.AppendLine("    .email-subject { font-weight: bold; font-size: 18px; margin-bottom: 10px; }");
            preview.AppendLine("    .email-info { color: #666; font-size: 14px; }");
            preview.AppendLine("    .email-body { padding: 20px; }");
            preview.AppendLine(
                "    .email-footer { padding: 15px; background-color: #f5f5f5; border-top: 1px solid #ddd; font-size: 12px; color: #666; text-align: center; }");
            preview.AppendLine("  </style>");
            preview.AppendLine("</head>");
            preview.AppendLine("<body>");
            preview.AppendLine("  <div class=\"email-preview\">");
            preview.AppendLine("    <div class=\"email-header\">");
            preview.AppendLine($"      <div class=\"email-subject\">{subject}</div>");
            preview.AppendLine("      <div class=\"email-info\">");
            preview.AppendLine($"        <strong>发件人:</strong> {config.DisplayName} &lt;{config.UserName}&gt;<br>");
            preview.AppendLine($"        <strong>发送时间:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            preview.AppendLine("      </div>");
            preview.AppendLine("    </div>");
            preview.AppendLine("    <div class=\"email-body\">");

            // 根据内容类型处理正文
            if (isBodyHtml)
            {
                preview.AppendLine(body);
            }
            else
            {
                preview.AppendLine($"<pre style=\"white-space: pre-wrap; word-wrap: break-word;\">{body}</pre>");
            }

            preview.AppendLine("    </div>");
            preview.AppendLine("    <div class=\"email-footer\">");
            preview.AppendLine("      此邮件为系统自动发送，请勿直接回复");
            preview.AppendLine("    </div>");
            preview.AppendLine("  </div>");
            preview.AppendLine("</body>");
            preview.AppendLine("</html>");

            return preview.ToString();
        }
        catch (Exception ex)
        {
            throw new EmailException("生成邮件预览失败", ex);
        }
    }

    private SmtpClient GetSmtpClient()
    {
        var config = GetConfigInfo();
        var smtpClient = new SmtpClient(config.SmtpServer, config.SmtpPort)
        {
            Credentials = new NetworkCredential(config.UserName, config.Password),
            EnableSsl = config.EnableSsl
        };
        return smtpClient;
    }

    public async Task SaveDraftAsync(
        string to,
        string subject,
        string body,
        SendEmailType type,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null,
        IEnumerable<string>? attachments = null,
        bool isBodyHtml = true)
    {
        try
        {
            var config = GetConfigInfo();

            // 创建邮件记录
            var email = new EmailBasic
            {
                Title = subject,
                Type = type,
                SenderEmail = config.UserName,
                SenderName = config.DisplayName,
                Status = EmailStatus.Draft
            };

            // 添加邮件内容
            email.EmailContent = new EmailContent
            {
                Content = body,
                Subject = subject,
                EmailId = email.Id,
                IsHtml = isBodyHtml,
                Recipient = to,
                CC = cc != null ? string.Join(";", cc) : null,
                BCC = bcc != null ? string.Join(";", bcc) : null,
                PlainTextContent = isBodyHtml ? body.StripHtml() : body
            };

            // 添加收件人记录
            email.EmailRecipients.Add(new EmailRecipient
            {
                RecipientEmail = to,
                Type = RecipientType.To,
                Status = DeliveryStatus.Pending,
                EmailRecordId = email.Id
            });

            // 添加抄送人记录
            if (cc != null)
            {
                foreach (var ccAddress in cc.Where(a => !string.IsNullOrEmpty(a)))
                {
                    email.EmailRecipients.Add(new EmailRecipient
                    {
                        RecipientEmail = ccAddress,
                        Type = RecipientType.CC,
                        Status = DeliveryStatus.Pending,
                        EmailRecordId = email.Id
                    });
                }
            }

            // 添加密送人记录
            if (bcc != null)
            {
                foreach (var bccAddress in bcc.Where(a => !string.IsNullOrEmpty(a)))
                {
                    email.EmailRecipients.Add(new EmailRecipient
                    {
                        RecipientEmail = bccAddress,
                        Type = RecipientType.BCC,
                        Status = DeliveryStatus.Pending,
                        EmailRecordId = email.Id
                    });
                }
            }

            // 添加附件记录
            if (attachments != null)
            {
                foreach (var attachment in attachments.Where(a => !string.IsNullOrEmpty(a) && File.Exists(a)))
                {
                    var fileInfo = new FileInfo(attachment);
                    email.EmailAttachments.Add(new EmailAttachment
                    {
                        ObjectKey = attachment,
                        FileName = fileInfo.Name,
                        FileSize = fileInfo.Length,
                        FileExtension = fileInfo.Extension,
                        ContentType = fileInfo.Extension.ToMimeType(),
                        StorageType = "local",
                        UploadTime = DateTime.UtcNow,
                        EmailRecordId = email.Id
                    });
                }
            }

            // 保存草稿到数据库
            await emailRepository.AddAsync(email);
            await emailRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new EmailException("保存邮件草稿失败", ex);
        }
    }

    public async Task<bool> VerifyEmailAsync(string email, string code, SendEmailType type)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new EmailException(nameof(email), "邮箱地址不能为空");
        }

        if (string.IsNullOrEmpty(code))
        {
            throw new EmailException(nameof(code), "验证码不能为空");
        }

        try
        {
            // 构建缓存键，格式：EMAIL_VERIFY_{类型}_{邮箱}
            var cacheKey = $"EMAIL_VERIFY_{type}_{email}";

            // 从缓存中获取验证码
            var cachedCode = await cacheService.GetAsync<string>(cacheKey);

            if (cachedCode == null)
            {
                // 验证码不存在或已过期
                return false;
            }

            // 比较验证码（不区分大小写）
            var isValid = string.Equals(cachedCode, code, StringComparison.OrdinalIgnoreCase);

            if (!isValid) return isValid;
            // 验证成功后删除缓存中的验证码，防止重复使用
            await cacheService.RemoveAsync(cacheKey);

            // 查询相关的邮件记录并更新状态
            var emailRecord = await emailRepository.FirstOrDefaultAsync(e =>
                e.Type == type &&
                e.EmailRecipients.Any(r => r.RecipientEmail == email) &&
                e.Status == EmailStatus.Sent);

            // 更新邮件记录状态
            var recipient = emailRecord?.EmailRecipients.FirstOrDefault(r => r.RecipientEmail == email);
            if (recipient is not null)
            {
                recipient.Status = DeliveryStatus.Read;
                await emailRepository.SaveChangesAsync();
            }

            return isValid;
        }
        catch (Exception ex)
        {
            throw new EmailException($"验证邮箱失败: {ex.Message}", email, ex);
        }
    }
}