namespace JackSite.Common.Configs;

public class RedisConfig
{
    public string InstanceName { get; set; } = string.Empty;

    public string ConnectionString { get; set; } = null!;
    
    public int DefaultDatabase { get; set; }
    public int ConnectTimeout { get; set; } = 5000;
    public bool AllowAdmin { get; set; }
    public int ConnectRetry { get; set; } = 3;
    public int SyncTimeout { get; set; } = 5000;
    public bool AbortOnConnectFail { get; set; }
}