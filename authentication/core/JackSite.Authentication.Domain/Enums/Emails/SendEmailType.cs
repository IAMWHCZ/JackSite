namespace JackSite.Authentication.Enums.Emails;

public enum SendEmailType : byte
{
    RegisterUser = 1,
    ResetPassword,
    ChangeEmail,
    ChangePhoneNumber,
    ChangePassword,
    ChangeProfile,
    ChangeSettings,
    ChangeRole,
    ChangeStatus,
    ChangeTimeZone,
    ChangeLanguage,
    ChangeTheme,
    ChangeTwoFactorAuth,
    ChangeNotifications,
    ChangeSmsNotifications,
    ChangeEmailNotifications
}