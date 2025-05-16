namespace JackSite.Http.Configuration;

public static class ConfigureEndpoint
{
    public static void AddEndpoints(this IServiceCollection service)
    {
        service.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true; // URL 自动转为小写
            options.AppendTrailingSlash = false; // 不自动添加尾部斜杠
        });

    }
}