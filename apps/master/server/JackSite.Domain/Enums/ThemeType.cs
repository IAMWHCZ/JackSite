using System.ComponentModel;

namespace JackSite.Domain.Enums;

public enum ThemeType:byte
{
    [Description("默认主题")]
    Default = 1,
    
    [Description("浅色主题")]
    Light,
    
    [Description("深色主题")]
    Dark
}