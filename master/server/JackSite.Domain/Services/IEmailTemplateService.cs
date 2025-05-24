namespace JackSite.Domain.Services;

public interface IEmailTemplateService
{
    /// <summary>
    /// 将 SendEmailType 转换为对应的 EmailTemplateType
    /// </summary>
    /// <param name="templateType"></param>
    /// <param name="language">语言代码，默认为英语</param>
    /// <param name="parameters"></param>
    /// <returns>对应的邮件模板类型</returns>
    /// <remarks>
    /// 此方法根据发送邮件的类型和指定的语言确定应使用的邮件模板类型。
    /// 不同的语言可能会映射到不同的模板类型。
    /// </remarks>
    Task<string> GetEmailTemplateAsync(EmailTemplateType templateType, string language, Dictionary<string, string> parameters);
}