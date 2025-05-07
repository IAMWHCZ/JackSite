var builder = DistributedApplication.CreateBuilder(args);

// 添加 PostgreSQL 服务
var postgres = builder.AddContainer("postgres", "postgres")
    .WithEnvironment("POSTGRES_USER", "cz")
    .WithEnvironment("POSTGRES_PASSWORD", "Cz18972621866")
    .WithEnvironment("POSTGRES_DB", "JackSiteDB")
    .WithVolume("postgres-data", "/var/lib/postgresql/data")
    .WithEndpoint(5432, 5432);

// 添加 Redis 服务
var redis = builder.AddRedis("redis");


// 添加 MinIO 服务
var minio = builder.AddContainer("minio", "quay.io/minio/minio")
    .WithEnvironment("MINIO_ROOT_USER", "admin")
    .WithEnvironment("MINIO_ROOT_PASSWORD", "Cz18972621866")
    .WithVolume("minio-data", "/data")
    .WithArgs("server", "/data", "--console-address", ":9001")
    .WithEndpoint( 9000,  9000, "minio-api")
    .WithEndpoint( 9001,  9001,"minio-console");

// 添加 HTTP 服务
var httpApi = builder.AddProject("jacksite-api", "../JackSite.Http/JackSite.Http.csproj")
    .WithEnvironment("ConnectionStrings__PGSQL", $"Host={postgres.GetEndpoint("tcp")};Database=JackSiteDB;Username=cz;Password=Cz18972621866")
    .WithReference(redis);


builder.Build().Run();