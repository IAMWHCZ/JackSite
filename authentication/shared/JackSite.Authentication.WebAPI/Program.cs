using JackSite.Authentication.Abstractions;
using JackSite.Authentication.WebAPI.Extensions;
using JackSite.Authentication.WebAPI.Middlewares;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.ApplySerilog();
    
    var configuration = builder.Configuration;
    
    builder.Services
        .AddInfrastructure(configuration)
        .AddApplication()
        .AddOpenApi()
        .AddApiModules()
        .AddClientCors()
        .AddHealthChecks();
    
    var app = builder.Build();
    
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<IApplicationDbInitializer>();
        await initializer.SeedAsync();
    }
    
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
        app.MapScalarApiReference(cfg =>
        {
            cfg.Title = "JackSite Authentication API";
        });
    }

    if (app.Environment.IsProduction())
    {
        app
            .UseHttpsRedirection()
            .UseHsts();
    }
    
    app.UseCors("DynamicCorsPolicy");
    
    app
        .UseSerilogConfig()
        .UseExceptionHandling()
        .MapApiModules();
    
    await app
        .RunAsync();
    
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}