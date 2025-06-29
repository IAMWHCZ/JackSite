namespace JackSite.Authentication.Enums;

public enum UserStatus:byte
{
    [Description("正常")]
    Active = 1,
    
    [Description("未激活")]
    Inactive,
    
    [Description("已锁定")]
    Locked,
    
    [Description("已禁用")]
    Banned
}