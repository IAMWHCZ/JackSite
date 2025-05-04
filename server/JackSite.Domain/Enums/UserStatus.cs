using System.ComponentModel;

namespace JackSite.Domain.Enums;

public enum UserStatus:byte
{
    [Description("正常")]
    Normal = 1,
    
    [Description("已锁定")]
    Locked,
    
    [Description("已禁用")]
    Banned
}