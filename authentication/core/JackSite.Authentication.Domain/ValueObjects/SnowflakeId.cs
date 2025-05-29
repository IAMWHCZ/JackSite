using JackSite.Authentication.Common;

namespace JackSite.Authentication.ValueObjects;

public static class SnowflakeId
{
    private static SnowflakeIdGenerator? _generator;

    public static void Configure(SnowflakeIdGenerator generator)
    {
        _generator = generator;
    }

    public static long NewId()
    {
        if (_generator == null)
            throw new InvalidOperationException("SnowflakeId 未初始化");
        return _generator.NextId();
    }
    
    private static long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - SnowflakeIdGenerator.Epoch).TotalMilliseconds;
    }
}