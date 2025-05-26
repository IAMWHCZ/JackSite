using JackSite.Authentication.Infrastructure.Extensions;
using Serilog;
using JackSite.Authentication.WebAPI.Configs;
using Scalar.AspNetCore;

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.ApplySerilog();

    var configuration = builder.Configuration;

    builder.Services.AddInfrastructure(configuration);

    builder.Services.AddOpenApi();
    
    builder.Services.AddHealthChecks();

    var app = builder.Build();

    app.UseSerilogConfig();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
        app.MapScalarApiReference(cfg =>
        {
            cfg.Title = "JackSite Authentication API";
        });
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}