var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

// 配置邮件选项
var emailOptions = new EmailOptions();
builder.Configuration.GetSection("Email").Bind(emailOptions);
builder.Services.AddSingleton(emailOptions);

// 注册邮件服务
builder.Services.AddScoped<IEmailService, EmailService>();

// 注册事件处理器
builder.Services.AddScoped<IEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();

builder.Services.AddJackSiteEntityFrameworkCore();

builder.Services.AddJackSiteDbContext<NotificationDbContext>(DbProvider.PostgreSQL, builder.Configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException());

var app = builder.Build();

app.MapGrpcService<EmailService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();