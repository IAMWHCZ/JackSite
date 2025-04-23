namespace JackSite.Infrastructure.Data;

public class MSSQLOptions
{
    public const string SectionName = "MSSQL";
    public string ConnectionString { get; set; } = string.Empty;
    public int CommandTimeout { get; set; } = 30;
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
}