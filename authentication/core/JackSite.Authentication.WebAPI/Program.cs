using JackSite.Authentication.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var app = builder.Build();

builder.Services.AddInfrastructure(configuration);

app.MapGet("/", () => "Hello World!");

app.Run();