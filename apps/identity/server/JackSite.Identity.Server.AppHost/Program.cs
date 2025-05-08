var builder = DistributedApplication.CreateBuilder(args);

var mysqlPassword = builder.AddParameter("MySqlPassword", "Cz18972621866");

var mysql = builder
            .AddMySql("mysql", mysqlPassword)
            .WithDataVolume()
            .AddDatabase("IdentityDB");

var garnet = builder.AddGarnet("cache")
                   .WithDataVolume(isReadOnly: false)
                   .WithPersistence(interval: TimeSpan.FromMinutes(5));

builder.AddProject<Projects.JackSite_Identity_Server>("identity-server")
                             .WithReference(garnet)
                             .WithReference(mysql)
                             .WaitFor(garnet)
                             .WaitFor(mysql);

await builder.Build().RunAsync();
