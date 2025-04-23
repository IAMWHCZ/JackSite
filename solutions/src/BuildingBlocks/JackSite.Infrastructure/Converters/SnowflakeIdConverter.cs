

namespace JackSite.Infrastructure.Converters;

public class SnowflakeIdConverter : ValueConverter<SnowflakeId, long>
{
    public SnowflakeIdConverter() 
        : base(
            v => (long)v,
            v => (SnowflakeId)v)
    {
    }
}