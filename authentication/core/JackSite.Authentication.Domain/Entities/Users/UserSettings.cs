namespace JackSite.Authentication.Entities.Users;

public class UserSettings : Entity
{
    [Required]
    public Guid UserId { get; private set; }
    
    [Required]
    public ThemeType Theme { get; private set; } = ThemeType.Default;
    
    [Required]
    public LanguageType Language { get; private set; } = LanguageType.English;
    
    [Required]
    public TimeZoneType TimeZone { get; private set; } = TimeZoneType.UTC;
    
    [Required]
    public bool EnableNotifications { get; private set; } = true;
    
    [Required]
    public bool EnableEmailNotifications { get; private set; } = true;
    
    [Required]
    public bool EnableSmsNotifications { get; private set; } = false;
    
    [Required]
    public bool EnableTwoFactorAuth { get; private set; } = false;
    
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? TwoFactorType { get; private set; }
    
    [Required]
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string DateFormat { get; private set; } = "yyyy-MM-dd";
    
    [Required]
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
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
    
    public void UpdateTimeZone(TimeZoneType timeZone)
    {
        TimeZone = timeZone;
    }
    
    public void UpdateDateTimeFormat(string dateFormat, string timeFormat)
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