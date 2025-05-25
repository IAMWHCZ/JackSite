using JackSite.Authentication.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();