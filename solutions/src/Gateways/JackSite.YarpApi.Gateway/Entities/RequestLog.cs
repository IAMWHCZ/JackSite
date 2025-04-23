using JackSite.Common.Domain;

namespace JackSite.YarpApi.Gateway.Entities;

public class RequestLog:EntityBase
{
    public string Path { get; set; } = null!;
    public string Method { get; set; } = null!;
    public string QueryString { get; set; } = string.Empty;
    public string RequestBody { get; set; } = string.Empty;
    public string? RequestHeaders { get; set; }
    public string? ResponseHeaders { get; set; }
    public string? ResponseBody { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StackTrace { get; set; }
    public string? ClientIp { get; set; }
    public string? UserAgent { get; set; }
    public DateTime RequestTime { get; set; }
    public DateTime ResponseTime { get; set; }
    public long ExecutionTime { get; set; } // 毫秒
    public string TargetService { get; set; } = null!; // 目标服务名称
}
