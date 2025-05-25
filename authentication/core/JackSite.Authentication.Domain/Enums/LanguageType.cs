using System.ComponentModel;

namespace JackSite.Authentication.Enums;

public enum LanguageType:byte
{
    [Description("英语 (en-US)")]
    English = 1,
    
    [Description("中文 (zh-CN)")]
    Chinese
}