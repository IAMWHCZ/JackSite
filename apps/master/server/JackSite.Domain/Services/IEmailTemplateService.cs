using JackSite.Domain.Enums;

namespace JackSite.Domain.Services;

public interface IEmailTemplateService
{
    Task<string> GetEmailTemplateAsync(EmailTemplateType templateType, string language, Dictionary<string, string> parameters);
}