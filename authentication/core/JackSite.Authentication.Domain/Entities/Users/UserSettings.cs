namespace JackSite.Authentication.Entities.Users;

public class UserSettings : Entity
{
    [Required]
    [Description("用户ID")]
    public Guid UserId { get; private set; }

    [Required]
    [Description("主题类型")]
    public ThemeType Theme { get; private set; } = ThemeType.Default;

    [Required]
    [Description("语言类型")]
    public LanguageType Language { get; private set; } = LanguageType.English;

    [Required]
    [Description("时区类型")]
    public TimeZoneType TimeZone { get; private set; } = TimeZoneType.UTC;

    [Required]
    [Description("启用通知")]
    public bool EnableNotifications { get; private set; } = true;

    [Required]
    [Description("启用邮件通知")]
    public bool EnableEmailNotifications { get; private set; } = true;

    [Required]
    [Description("启用短信通知")]
    public bool EnableSmsNotifications { get; private set; } = false;

    [Required]
    [Description("启用双因子认证")]
    public bool EnableTwoFactorAuth { get; private set; } = false;

    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    [Description("双因子认证类型")]
    public string? TwoFactorType { get; private set; }

    [Required]
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    [Description("日期格式")]
    public string DateFormat { get; private set; } = "yyyy-MM-dd";

    [Required]
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    [Description("时间格式")]
    public string TimeFormat { get; private set; } = "HH:mm:ss";
    // 导航属性
    public virtual UserBasic User { get; private set; } = null!;
    
    // 供EF Core使用的私有构造函数
    private UserSettings() { }
    
    // 领域构造函数
    public UserSettings(Guid userId)
    {
        UserId = userId;
    }
    
    // 领域行为
    public void UpdateTheme(ThemeType theme)
    {
        Theme = theme;
    }
    
    public void UpdateLanguage(LanguageType language)
    {
        Language = language;
    }
    
    public void UpDateTimeOffsetZone(TimeZoneType timeZone)
    {
        TimeZone = timeZone;
    }
    
    public void UpdateDateTimeOffsetFormat(string dateFormat, string timeFormat)
    {
        if (string.IsNullOrWhiteSpace(dateFormat))
            throw new ArgumentException("Date format cannot be empty", nameof(dateFormat));
            
        if (string.IsNullOrWhiteSpace(timeFormat))
            throw new ArgumentException("Time format cannot be empty", nameof(timeFormat));
            
        DateFormat = dateFormat;
        TimeFormat = timeFormat;
    }
    
    public void ConfigureNotifications(bool enableNotifications, bool enableEmail, bool enableSms)
    {
        EnableNotifications = enableNotifications;
        EnableEmailNotifications = enableEmail;
        EnableSmsNotifications = enableSms;
    }
    
    public void ConfigureTwoFactorAuth(bool enable, string? type = null)
    {
        EnableTwoFactorAuth = enable;
        TwoFactorType = enable ? type : null;
    }
}