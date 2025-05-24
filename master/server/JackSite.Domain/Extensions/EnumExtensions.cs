using JackSite.Domain.Enums;

namespace JackSite.Domain.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// 将 SendEmailType 转换为对应的 EmailTemplateType
    /// </summary>
    /// <param name="sendEmailType">发送邮件类型</param>
    /// <returns>对应的邮件模板类型</returns>
    public static EmailTemplateType ToEmailTemplateType(this SendEmailType sendEmailType)
    {
        return sendEmailType switch
        {
            SendEmailType.RegisterUser => EmailTemplateType.Register,
            SendEmailType.ResetPassword => EmailTemplateType.ForgetPassword,
            SendEmailType.ChangeEmail => EmailTemplateType.UpdateEmail,
            // 其他类型可能需要映射到默认模板或特定模板
            _ => EmailTemplateType.Register // 默认使用注册模板
        };
    }

    /// <summary>
    /// 将 EmailTemplateType 转换为对应的 SendEmailType
    /// </summary>
    /// <param name="emailTemplateType">邮件模板类型</param>
    /// <returns>对应的发送邮件类型</returns>
    public static SendEmailType ToSendEmailType(this EmailTemplateType emailTemplateType)
    {
        return emailTemplateType switch
        {
            EmailTemplateType.Register => SendEmailType.RegisterUser,
            EmailTemplateType.ForgetPassword => SendEmailType.ResetPassword,
            EmailTemplateType.UpdateEmail => SendEmailType.ChangeEmail,
            EmailTemplateType.Login => SendEmailType.RegisterUser, // 登录可能使用注册用户类型
            _ => SendEmailType.RegisterUser // 默认使用注册用户类型
        };
    }
}