namespace JackSite.Authentication.Infrastructure.Options;

public class SnowflakeIdOption
{
    /// <summary>
    /// 机器ID（0-31）
    /// </summary>
    public long MachineId { get; set; }

    /// <summary>
    /// 数据中心ID（0-31）
    /// </summary>
    public long DatacenterId { get; set; }
}