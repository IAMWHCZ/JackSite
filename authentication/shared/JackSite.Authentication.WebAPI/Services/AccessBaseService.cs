using JackSite.Authentication.Abstractions.Services;
using JackSite.Shared.Models;

namespace JackSite.Authentication.WebAPI.Services;

public class AccessBaseService(
    IHttpContextAccessor contextAccessor
    ) 
    : IAccessBaseService
{
    public string? GetHeaderValue(string headerName)
    {
        var context = contextAccessor.HttpContext;
        if (context == null)
            return null;
            
        return context.Request.Headers.TryGetValue(headerName, out var values) ? values.ToString() : null;
    }
    
    public HttpFromBase GetCurrentFormBase()
    {
        var currentFormUser = new HttpFromBase();
        var context = contextAccessor.HttpContext;
        if (context == null)
        {
            return currentFormUser;  // 返回默认对象
        }
        
        currentFormUser.Language = GetHeaderValue("Accept-Language") ?? "en-US";
        currentFormUser.TimeZone = GetHeaderValue("Time-Zone");
        
        return currentFormUser;
    }
}