

namespace JackSite.Infrastructure.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly string _templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

    public async Task<string> GetEmailTemplateAsync(EmailTemplateType templateType, string language,
        Dictionary<string, string> parameters)
    {
        // 确定模板文件名
        var templateFileName = GetTemplateFileName(templateType, language);
        var templatePath = Path.Combine(_templatePath, templateFileName);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Email template not found: {templatePath}");
        }

        // 读取模板内容
        var templateContent = await File.ReadAllTextAsync(templatePath);

        // 替换参数
        templateContent = parameters.Aggregate(templateContent,
            (current, param) => current.Replace($"{{{{{param.Key}}}}}", param.Value));

        // 替换验证码
        if (parameters.TryGetValue("VerificationCode", out var code))
        {
            // 找到验证码显示区域并替换
            templateContent = Regex.Replace(
                templateContent,
                """<div\s+style="display: inline-block; background-color: #F3F4F6; border-radius: 8px; padding: 20px 40px; letter-spacing: 8px; font-size: 28px; font-weight: 600; color: #1F2937; border: 1px solid #E5E7EB;">\s*123456\s*</div>""",
                $"""<div style="display: inline-block; background-color: #F3F4F6; border-radius: 8px; padding: 20px 40px; letter-spacing: 8px; font-size: 28px; font-weight: 600; color: #1F2937; border: 1px solid #E5E7EB;">{code}</div>""");
        }

        return templateContent;
    }

    private static string GetTemplateFileName(EmailTemplateType templateType, string language)
    {
        language = language.Equals("zh", StringComparison.CurrentCultureIgnoreCase) || language.Equals("cn", StringComparison.CurrentCultureIgnoreCase) ? "cn" : "en";

        return templateType switch
        {
            EmailTemplateType.Register => $"register-email-{language}.html",
            EmailTemplateType.Login => $"login-email-{language}.html",
            EmailTemplateType.ForgetPassword => $"forget-password-{language}.html",
            EmailTemplateType.UpdateEmail => $"update-email-{language}.html",
            _ => throw new ArgumentException($"Unsupported email template type: {templateType}")
        };
    }
}