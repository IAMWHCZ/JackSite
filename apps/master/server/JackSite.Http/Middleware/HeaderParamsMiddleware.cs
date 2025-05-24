using JackSite.Domain.Services;
using JackSite.Infrastructure.Services;

namespace JackSite.Http.Middleware;

public class HeaderParamsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IRequestHeaderService requestHeaderService)
    {
        // 确保在每个请求开始时刷新请求头参数
        if (requestHeaderService is RequestHeaderService service)
        {
            service.RefreshHeaderParams();
        }

        await next(context);
    }
}