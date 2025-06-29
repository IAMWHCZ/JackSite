using System.ComponentModel;

namespace JackSite.Authentication.Entities.Logs;

public class OperationLog : BaseLogEntity
{
    [Description("API名称")]
    public string ApiName { get; set; } = null!;

    [Description("操作描述")]
    public string? Description { get; set; }

    [Description("是否需要授权")]
    public bool IsAuthorization { get; set; }

    [Description("用户ID")]
    public Guid? UserId { get; set; }

    [Description("IP地址")]
    public string? IpAddress { get; set; }

    [Description("用户代理")]
    public string? UserAgent { get; set; }

    [Description("浏览器")]
    public string? Browser { get; set; }

    [Description("操作系统")]
    public string? Os { get; set; }
}