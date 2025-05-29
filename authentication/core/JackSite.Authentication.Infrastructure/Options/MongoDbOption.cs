namespace JackSite.Authentication.Infrastructure.Options;

public class MongoDbOption
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}