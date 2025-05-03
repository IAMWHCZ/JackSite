using JackSite.Domain.Services;

namespace JackSite.Http.Middleware;

public class PermissionAuthorizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        // 从请求中获取用户ID（假设已经通过身份验证中间件设置）
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && long.TryParse(userIdClaim.Value, out var userId))
            {
                // 获取当前请求的路径和方法
                var path = context.Request.Path.Value?.ToLower();
                var method = context.Request.Method.ToUpper();
                
                // 根据路径和方法确定所需的权限代码
                var requiredPermission = GetRequiredPermission(path, method);
                
                if (!string.IsNullOrEmpty(requiredPermission))
                {
                    // 检查用户是否拥有所需权限
                    var hasPermission = await userService.HasPermissionAsync(userId, requiredPermission);
                    
                    if (!hasPermission)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new { error = "Insufficient permissions" });
                        return;
                    }
                }
            }
        }
        
        await next(context);
    }
    
    private string? GetRequiredPermission(string? path, string method)
    {
        // 这里可以实现一个基于路径和方法的权限映射逻辑
        // 例如，可以使用一个字典或配置文件来存储路径-方法-权限的映射关系
        
        // 简单示例：
        if (path == null)
        {
            return null;
        }

        if (path.StartsWith("/api/users") && method == "GET")
        {
            return "users.read";
        }

        if (path.StartsWith("/api/users") && method is "POST" or "PUT" or "DELETE")
        {
            return "users.write";
        }

        if (path.StartsWith("/api/roles") && method == "GET")
        {
            return "roles.read";
        }

        if (path.StartsWith("/api/roles") && method is "POST" or "PUT" or "DELETE")
        {
            return "roles.write";
        }

        return null;
    }
}

// 扩展方法，用于在Startup中注册中间件
public static class PermissionAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UsePermissionAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PermissionAuthorizationMiddleware>();
    }
}