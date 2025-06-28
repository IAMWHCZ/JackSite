namespace JackSite.Authentication.Entities.Users;

public class UserSecurityLog : Entity
{
    [Required]
    public Guid UserId { get; private set; }
    
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Action { get; private set; } = string.Empty;
    
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? IpAddress { get; private set; }
    
    [MaxLength(500)]
    [Column(TypeName = "varchar(500)")]
    public string? UserAgent { get; private set; }
    
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? Browser { get; private set; }
    
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? OperatingSystem { get; private set; }
    
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? DeviceType { get; private set; }
    
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? Location { get; private set; }
    
    [Required]
    public bool IsSuccessful { get; private set; }
    
    [MaxLength(200)]
    [Column(TypeName = "varchar(200)")]
    public string? FailureReason { get; private set; }
    
    [Required]
    public DateTime Timestamp { get; private set; }
    
    // 导航属性
    public virtual UserBasic User { get; private set; } = null!;
    
    // 供EF Core使用的私有构造函数
    private UserSecurityLog() { }
    
    // 领域构造函数
    public UserSecurityLog(
        Guid userId, 
        string action, 
        string? ipAddress = null, 
        string? userAgent = null,
        bool isSuccessful = true,
        string? failureReason = null)
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new ArgumentException("Action cannot be empty", nameof(action));
            
        UserId = userId;
        Action = action;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        IsSuccessful = isSuccessful;
        FailureReason = failureReason;
        Timestamp = DateTime.UtcNow;
        
        // 解析用户代理字符串
        if (!string.IsNullOrWhiteSpace(userAgent))
        {
            ParseUserAgent(userAgent);
        }
    }
    
    private void ParseUserAgent(string userAgent)
    {
        // 简单的用户代理解析逻辑
        // 在实际应用中，可以使用专门的库来解析用户代理
        Browser = DetermineBrowser(userAgent);
        OperatingSystem = DetermineOperatingSystem(userAgent);
        DeviceType = DetermineDeviceType(userAgent);
    }
    
    private static string? DetermineBrowser(string userAgent)
    {
        // 简化的浏览器检测逻辑
        if (userAgent.Contains("Chrome"))
            return "Chrome";
        if (userAgent.Contains("Firefox"))
            return "Firefox";
        if (userAgent.Contains("Safari"))
            return "Safari";
        if (userAgent.Contains("Edge"))
            return "Edge";
        if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            return "Internet Explorer";
            
        return null;
    }
    
    private static string? DetermineOperatingSystem(string userAgent)
    {
        // 简化的操作系统检测逻辑
        if (userAgent.Contains("Windows"))
            return "Windows";
        if (userAgent.Contains("Mac"))
            return "MacOS";
        if (userAgent.Contains("Linux"))
            return "Linux";
        if (userAgent.Contains("Android"))
            return "Android";
        if (userAgent.Contains("iOS") || userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
            return "iOS";
            
        return null;
    }
    
    private static string? DetermineDeviceType(string userAgent)
    {
        // 简化的设备类型检测逻辑
        if (userAgent.Contains("Mobile"))
            return "Mobile";
        if (userAgent.Contains("Tablet") || userAgent.Contains("iPad"))
            return "Tablet";
            
        return "Desktop";
    }
    
    public void SetLocation(string location)
    {
        Location = location;
    }
}