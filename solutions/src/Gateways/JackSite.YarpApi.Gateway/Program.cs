using Carter;
using JackSite.Common.Configs;
using JackSite.Infrastructure.Data;
using JackSite.Infrastructure.Extensions;
using JackSite.YarpApi.Gateway.Data;
using JackSite.YarpApi.Gateway.Middleware;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using JackSite.YarpApi.Gateway.Extensions;
using JackSite.YarpApi.Gateway.Jobs;

SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

// 添加数据库上下文

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

var dbOptions = builder.Configuration.GetOptions<MSSQLOptions>("SqLite");
builder.Services.ConfigureAndValidate<MSSQLOptions>(builder.Configuration, MSSQLOptions.SectionName);
var corsConfig = builder.Configuration.GetSection("CorsPolicy").Get<CorsPolicyConfig>();

builder.Host.AddDefaultSerilog("JackSite.YarpApi.Gateway");

builder.Services.AddDbContext<GatewayDbContext>(opt =>
{
    opt.UseSqlite(dbOptions.ConnectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(dbOptions.CommandTimeout);
    });

    if (!builder.Environment.IsDevelopment()) return;
    opt.EnableDetailedErrors(dbOptions.EnableDetailedErrors);
    opt.EnableSensitiveDataLogging(dbOptions.EnableSensitiveDataLogging);
});

builder.Services.AddInfrastructure(builder.Configuration, typeof(Program).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(corsConfig!.Origins.ToArray())
            .WithMethods(corsConfig.Methods.ToArray())
            .WithHeaders(corsConfig.Headers.ToArray())
            .WithExposedHeaders(corsConfig.ExposedHeaders.ToArray());

        if (corsConfig.AllowCredentials)
        {
            policy.AllowCredentials();
        }
        else
        {
            policy.DisallowCredentials();
        }
    });
});

// 配置反向代理
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// 添加后台服务
builder.Services.AddHostedService<RequestLogCleanupService>();

var app = builder.Build(); 

app.UseCors("CorsPolicy");

// 确保数据库存在
await app.EnsureDatabaseAsync(app.Logger,builder.Configuration);

// 添加请求日志中间件（在其他中间件之前）
app.UseRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference(opt =>
    {
        opt.Title = builder.Configuration["Scalar:Title"];
        opt.HideModels = true;
    });
}

app.UseDefaultSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapHealthChecks("/health");

app.MapCarter();

await app.RunAsync();

