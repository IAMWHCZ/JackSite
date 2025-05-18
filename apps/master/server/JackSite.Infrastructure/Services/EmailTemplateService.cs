using System.IO;
using System.Text.RegularExpressions;
using JackSite.Domain.Enums;
using JackSite.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace JackSite.Infrastructure.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly string _templatePath;
    private readonly IConfiguration _configuration;

    public EmailTemplateService(IConfiguration configuration)
    {
        _configuration = configuration;
        _templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
    }

    public async Task<string> GetEmailTemplateAsync(EmailTemplateType templateType, string language, Dictionary<string, string> parameters)
    {
        // 确定模板文件名
        string templateFileName = GetTemplateFileName(templateType, language);
        string templatePath = Path.Combine(_templatePath, templateFileName);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Email template not found: {templatePath}");
        }

        // 读取模板内容
        string templateContent = await File.ReadAllTextAsync(templatePath);

        // 替换参数
        foreach (var param in parameters)
        {
            templateContent = templateContent.Replace($"{{{{{param.Key}}}}}", param.Value);
        }

        // 替换验证码
        if (parameters.TryGetValue("VerificationCode", out string code))
        {
            // 找到验证码显示区域并替换
            templateContent = Regex.Replace(
                templateContent,
                @"<div\s+style=""display: inline-block; background-color: #F3F4F6; border-radius: 8px; padding: 20px 40px; letter-spacing: 8px; font-size: 28px; font-weight: 600; color: #1F2937; border: 1px solid #E5E7EB;"">\s*123456\s*</div>",
                $@"<div style=""display: inline-block; background-color: #F3F4F6; border-radius: 8px; padding: 20px 40px; letter-spacing: 8px; font-size: 28px; font-weight: 600; color: #1F2937; border: 1px solid #E5E7EB;"">{code}</div>");
        }

        return templateContent;
    }

    private string GetTemplateFileName(EmailTemplateType templateType, string language)
    {
        language = language?.ToLower() == "zh" || language?.ToLower() == "cn" ? "cn" : "en";
        
        return templateType switch
        {
            EmailTemplateType.Register => $"register-email-{language}.html",
            EmailTemplateType.Login => $"login-email-{language}.html",
            EmailTemplateType.ForgetPassword => $"forget-passowrd-{language}.html",
            EmailTemplateType.UpdateEmail => $"update-email-{language}.html",
            _ => throw new ArgumentException($"Unsupported email template type: {templateType}")
        };
    }
}