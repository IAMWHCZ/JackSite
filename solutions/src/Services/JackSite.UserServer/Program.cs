using JackSite.Infrastructure.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // 添加Serilog配置
    builder.Host.AddDefaultSerilog(builder.Environment.ApplicationName);

    // 配置数据库选项
    var dbOptions = builder.Configuration.GetOptions<MSSQLOptions>(MSSQLOptions.SectionName);
    builder.Services.ConfigureAndValidate<MSSQLOptions>(builder.Configuration, MSSQLOptions.SectionName);

    // 添加DbContext
    builder.Services.AddDbContext<UserDbContext>(options =>
    {
        options.UseSqlServer(dbOptions.ConnectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(3);
            sqlOptions.CommandTimeout(dbOptions.CommandTimeout);
        });

        if (!builder.Environment.IsDevelopment()) return;
        options.EnableDetailedErrors(dbOptions.EnableDetailedErrors);
        options.EnableSensitiveDataLogging(dbOptions.EnableSensitiveDataLogging);
    });
    // 添加仓储
    builder.Services.AddInfrastructure(builder.Configuration);
    // 添加健康检查
    builder.Services.AddDefaultHealthChecks(builder.Configuration);

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // 添加Serilog请求日志记录
    app.UseDefaultSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseDefaultHealthChecks();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}