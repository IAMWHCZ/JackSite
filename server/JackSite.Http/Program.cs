using JackSite.Http.Middleware;
using JackSite.Infrastructure.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddApplication();

builder.ConfigureSerilog(configuration);


builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseLogContext();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



app.Run();