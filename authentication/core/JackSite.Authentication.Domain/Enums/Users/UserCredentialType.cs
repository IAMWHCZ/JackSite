namespace JackSite.Authentication.Enums.Users;

public enum UserCredentialType: byte
{
    [Description("密码")]
    Password = 1,
    [Description("一次性密码")]
    Otp = 2, 
    [Description("证书")]
    Certificate = 3 
}