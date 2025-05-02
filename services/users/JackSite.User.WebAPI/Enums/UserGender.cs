namespace JackSite.User.WebAPI.Enums;

public enum UserGender:byte
{
    [Description("女")]
    Female = 1,
    [Description("男")]
    Male,
    [Description("保密")]
    Confidential,
    [Description("未知")]
    Unknown,
    [Description("其他")]
    Other,
    
}