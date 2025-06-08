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
        .AddHealthChecks();

    var app = builder.Build();

    app.UseSerilogConfig()
        .UseExceptionHandling();


    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.MapOpenApi();
        app.MapScalarApiReference(cfg => { cfg.Title = "JackSite Authentication API"; });
    }

    if (app.Environment.IsProduction())
    {
        app.UseHttpsRedirection()
            .UseHsts();
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