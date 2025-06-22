var builder = DistributedApplication.CreateBuilder(args);

// 使用环境变量或配置文件存储敏感信息
var mysqlPassword = builder.Configuration["MySqlPassword"] ?? "DefaultDevPassword";

var mysql = builder
    .AddMySql("jacksite-authentication")
    .WithImage("mysql:8.0")
    .WithVolume("jacksite-auth-data", "/var/lib/mysql")
    .WithEnvironment("MYSQL_ROOT_PASSWORD", mysqlPassword)
    .WithEnvironment("MYSQL_DATABASE", "JackSiteAuthenticationDB")
    .WithEndpoint(name: "mysql", port: 3306, targetPort: 3306)
    .WithLifetime(ContainerLifetime.Persistent);

var seq = builder.AddSeq("seq")
    .ExcludeFromManifest()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithDataVolume();

var garnet = builder.AddGarnet("garnet");

var db = mysql.AddDatabase("JackSiteAuthenticationDB");

await builder.Build().RunAsync();