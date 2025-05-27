var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder
    .AddMySql("jacksite-authentication")
    .WithImage("mysql:8.0")  // 指定MySQL 8.0版本，这是当前最稳定的免费LTS版本
    .WithVolume("jacksite-auth-data", "/var/lib/mysql")
    .WithEnvironment("MYSQL_ROOT_PASSWORD", "Cz18972621866")
    .WithEnvironment("MYSQL_DATABASE", "JackSiteAuthenticationDB")
    .WithEndpoint(name: "mysql", port: 3306, targetPort: 3306)
    .WithLifetime(ContainerLifetime.Persistent);

var db = mysql.AddDatabase("JackSiteAuthenticationDB");

builder.AddProject("JackSiteAuthenticationWebAPI",
    "../core/JackSite.Authentication.WebAPI/JackSite.Authentication.WebAPI.csproj");

await builder.Build().RunAsync();