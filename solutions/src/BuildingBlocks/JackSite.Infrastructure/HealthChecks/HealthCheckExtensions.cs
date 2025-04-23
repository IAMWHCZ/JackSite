namespace JackSite.Infrastructure.HealthChecks;

public static class HealthCheckExtensions
{
    public static void AddDefaultHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddSqlServer(
                configuration["Database:ConnectionString"] ?? throw new InvalidOperationException("Database connection string not found."),
                name: "sql-check",
                tags: ["sql"]);
    }

    public static void UseDefaultHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(x => new
                    {
                        name = x.Key,
                        status = x.Value.Status.ToString(),
                        description = x.Value.Description
                    })
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        });
    }
}