using JackSite.Authentication.Enums.Emails;

namespace JackSite.Authentication.Const;

public static class EmailTemplateConst
{
    public const string ForgetPasswordCn = "forget-cn.html";

    public const string ForgetPasswordEn = "forget-en.html";

    public const string RegisterCn = "register-cn.html";

    public const string RegisterEn = "register-en.html";

    public const string SignInConfirmCn = "signin-confirm-cn.html";

    public const string SignInConfirmEn = "signin-confirm-en.html";

    /// <summary>
    /// 获取指定类型邮件的模板名称
    /// </summary>
    /// <param name="type">邮件类型</param>
    /// <param name="isEnglish">是否为英文模板，默认为英文</param>
    /// <returns>模板文件名</returns>
    public static string GetTemplateByType(SendEmailType type, bool isEnglish = true)
    {
        return type switch
        {
            SendEmailType.RegisterUser => isEnglish ? RegisterEn : RegisterCn,
            SendEmailType.ResetPassword => isEnglish ? ForgetPasswordEn : ForgetPasswordCn,
            SendEmailType.ChangePassword => isEnglish ? ForgetPasswordEn : ForgetPasswordCn,
            SendEmailType.ChangeEmail => isEnglish ? SignInConfirmEn : SignInConfirmCn,
            SendEmailType.ChangePhoneNumber => isEnglish ? SignInConfirmEn : SignInConfirmCn,
            // 对于其他验证类型，可以使用通用模板或默认使用注册模板
            _ => isEnglish ? RegisterEn : RegisterCn
        };
    }

    /// <summary>
    /// 获取指定类型邮件的模板名称（不含扩展名）
    /// </summary>
    /// <param name="type">邮件类型</param>
    /// <param name="isEnglish">是否为英文模板，默认为英文</param>
    /// <returns>模板名称（不含扩展名）</returns>
    public static string GetTemplateNameByType(SendEmailType type, bool isEnglish = true)
    {
        var template = GetTemplateByType(type, isEnglish);
        return template;
    }

    /// <summary>
    /// 获取指定类型邮件的默认主题
    /// </summary>
    /// <param name="type">邮件类型</param>
    /// <param name="isEnglish">是否为英文主题，默认为英文</param>
    /// <returns>邮件主题</returns>
    public static string GetSubjectByType(SendEmailType type, bool isEnglish = true)
    {
        if (isEnglish)
        {
            return type switch
            {
                SendEmailType.RegisterUser => "Email Verification Code for Registration",
                SendEmailType.ResetPassword => "Password Reset Verification Code",
                SendEmailType.ChangePassword => "Password Change Verification Code",
                SendEmailType.ChangeEmail => "Email Change Verification Code",
                SendEmailType.ChangePhoneNumber => "Phone Number Change Verification",
                SendEmailType.ChangeTwoFactorAuth => "Two-Factor Authentication Verification",
                _ => "JackSite Verification Code"
            };
        }

        return type switch
        {
            SendEmailType.RegisterUser => "注册验证码",
            SendEmailType.ResetPassword => "密码重置验证码",
            SendEmailType.ChangePassword => "密码修改验证码",
            SendEmailType.ChangeEmail => "邮箱变更验证码",
            SendEmailType.ChangePhoneNumber => "手机号变更验证码",
            SendEmailType.ChangeTwoFactorAuth => "双因素认证验证码",
            _ => "JackSite 验证码"
        };
    }
}