using System.ComponentModel;

namespace JackSite.Identity.Server.Enums;

public enum LanguageModuleType
{
    [Description("语言选择器")]
    LanguageSelect = 1,

    [Description("登录方式")]
    LoginMethod = 2,
}
