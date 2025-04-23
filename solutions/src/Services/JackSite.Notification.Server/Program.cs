using JackSite.Notification.Server.Configs;
using JackSite.Notification.Server.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddCarter();
builder.Services.AddHealthChecks();
var dbOptions = configuration.GetOptions<MSSQLOptions>(MSSQLOptions.SectionName);
builder.Services.ConfigureAndValidate<MSSQLOptions>(configuration, MSSQLOptions.SectionName);
var corsConfig = configuration.GetSection("CorsPolicy").Get<CorsPolicyConfig>();
builder.Host.AddDefaultSerilog(configuration["System:ApplicationName"] ?? throw new InvalidOperationException("未加载到配置项"));
builder.Services.AddSignalR();

builder.Services.AddDbContext<NotificationDbContext>(opt =>
{
    opt.UseSqlServer(dbOptions.ConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(3);
        sqlOptions.CommandTimeout(dbOptions.CommandTimeout);
    });

    if (!builder.Environment.IsDevelopment()) return;
    opt.EnableDetailedErrors(dbOptions.EnableDetailedErrors);
    opt.EnableSensitiveDataLogging(dbOptions.EnableSensitiveDataLogging);
});

builder.Services.AddHostedService<EmailRetryService>();

builder.Services.AddInfrastructure(builder.Configuration, typeof(Program).Assembly);

builder.Services.AddServices(typeof(Program).Assembly);

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

// 配置定时消息服务
builder.Services.Configure<ScheduledMessageConfig>(
    builder.Configuration.GetSection("ScheduledMessage"));

builder.Services.AddHostedService<ScheduledMessageProcessorService>();

var app = builder.Build();


app.UseDefaultSerilogRequestLogging();

app.MapHub<NotificationHub>("/hubs/notification");

app.UseExceptionHandler("/error");

app.MapHealthChecks("/health");

app.MapCarter();

await app.RunAsync();