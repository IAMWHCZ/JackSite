

namespace JackSite.Shared.EntityFrameworkCore.Converters;

/// <summary>
/// SnowflakeId 值转换器
/// </summary>
public class SnowflakeIdValueConverter : ValueConverter<SnowflakeId, long>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SnowflakeIdValueConverter() 
        : base(
            v => v.ToInt64(),
            v => new SnowflakeId(v))
    {
    }
}