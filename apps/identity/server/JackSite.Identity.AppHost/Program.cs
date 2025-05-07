var builder = DistributedApplication.CreateBuilder(args);
var mysqlPassword = builder.AddParameter("MySqlPassword", "Cz18972621866");
var redis = builder.AddGarnet("cache")
                   .WithDataVolume(isReadOnly:false)
                   .WithPersistence(
                        interval: TimeSpan.FromMinutes(5),
                       keysChangedThreshold: 100);
var mysql = builder
            .AddMySql("mysql",mysqlPassword)
            .WithDataVolume()
            .AddDatabase("IdentityDB");

var identityServer =  builder.AddProject<Projects.JackSite_Identity_Server>("identity_server")
                             .WithReference(redis)
                             .WithReference(mysql)
                             .WaitFor(redis)
                             .WaitFor(mysql);


await builder.Build().RunAsync();
