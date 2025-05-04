using System.ComponentModel;

namespace JackSite.Domain.Enums;

public enum TimeZoneType:byte
{
    [Description("协调世界时 (UTC+0)")]
    UTC = 1,
    
    [Description("格林尼治标准时间 (GMT+0)")]
    GMT,
    
    [Description("东部标准时间 (UTC-5)")]
    EST,
    
    [Description("中部标准时间 (UTC-6)")]
    CST,
    
    [Description("山地标准时间 (UTC-7)")]
    MST,
    
    [Description("太平洋标准时间 (UTC-8)")]
    PST,
    
    [Description("中欧时间 (UTC+1)")]
    CET,
    
    [Description("东欧时间 (UTC+2)")]
    EET,
    
    [Description("日本标准时间 (UTC+9)")]
    JST,
    
    [Description("澳大利亚东部标准时间 (UTC+10)")]
    AEST,
    
    [Description("中国标准时间 (UTC+8)")]
    CST_China
}