using System.Threading.Tasks;
using JackSite.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace JackSite.Http.Middleware;

public class HeaderParamsMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderParamsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IRequestHeaderService requestHeaderService)
    {
        // 确保在每个请求开始时刷新请求头参数
        if (requestHeaderService is RequestHeaderService service)
        {
            service.RefreshHeaderParams();
        }

        await _next(context);
    }
}