
namespace JackSite.Shared.EntityFrameworkCore.Providers;

/// <summary>
/// 数据库提供程序类型
/// </summary>
public enum DbProvider
{
    /// <summary>
    /// PostgreSQL
    /// </summary>
    [Description("PostgreSQL")]
    PostgreSQL = 1 ,
    
    /// <summary>
    /// SQL Server
    /// </summary>
    [Description("SQL Server")]
    SqlServer,
    
    /// <summary>
    /// MySQL
    /// </summary>
    [Description("MySQL")]
    MySQL,
    
    /// <summary>
    /// SQLite
    /// </summary>
    [Description("SQLite")]
    SQLite
}