var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services
    .AddMinimalApis()
    .AddJackSiteLogging(configuration,"Logging")
    .AddSnowflakeIdGenerator(configuration["Application:WorkerId"].ToLong())
    .AddJackSiteCQRS(Assembly.GetExecutingAssembly())
    .AddJackSiteEntityFrameworkCore()
    .AddJackSiteDbContext<UserDbContext>(DbProvider.SqlServer,configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException());


builder.Host.UseJackSiteSerilog("JackSiteLogging");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "用户服务";
        opt.HeadContent = "用户服务";
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>()
    .UseMiddleware<RequestLoggingMiddleware>();

app.MapMinimalApiEndpoints();

await app.RunAsync();