using JackSite.Authentication.Abstractions.Services;
using JackSite.Shared.Models;

namespace JackSite.Authentication.WebAPI.Services;

public class HttpContextAccessorService(
    IHttpContextAccessor contextAccessor
    ) 
    : IHttpContextAccessorService
{
    public string? GetHeaderValue(string headerName)
    {
        var context = contextAccessor.HttpContext;
        if (context == null)
            return null;
            
        return context.Request.Headers.TryGetValue(headerName, out var values) ? values.ToString() : null;
    }
    
    public HttpFromBase GetCurrentFormUser()
    {
        var currentFormUser = new HttpFromBase();
        var context = contextAccessor.HttpContext;
        if (context == null)
        {
            return currentFormUser;  // 返回默认对象
        }
        
        currentFormUser.UserAgent = GetHeaderValue("User-Agent");
        currentFormUser.ClientIp = GetHeaderValue("X-Forwarded-For") ?? context.Connection.RemoteIpAddress?.ToString();
        currentFormUser.Language = GetHeaderValue("Accept-Language") ?? "en-US";
        currentFormUser.Referer = GetHeaderValue("Referer");
        currentFormUser.TimeZone = GetHeaderValue("Time-Zone");
        currentFormUser.IsMobileDevice = context.Request.Headers.ContainsKey("Is-Mobile-Device") &&
                                         bool.TryParse(GetHeaderValue("Is-Mobile-Device"), out var isMobile) && isMobile;
        
        return currentFormUser;
    }
}