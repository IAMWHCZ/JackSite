namespace JackSite.Shared.Caching.Services;

/// <summary>
/// 缓存键生成器
/// </summary>
public static class CacheKeyGenerator
{
    /// <summary>
    /// 生成缓存键
    /// </summary>
    public static string Generate(string prefix, params object[] parameters)
    {
        if (parameters.Length == 0)
            return prefix;
            
        var key = new StringBuilder(prefix);
        
        foreach (var param in parameters)
        {
            key.Append(':');

            switch (param)
            {
                case string str:
                    key.Append(str);
                    break;
                case DateTime dt:
                    key.Append(dt.ToString("yyyyMMddHHmmss"));
                    break;
                case DateTimeOffset dto:
                    key.Append(dto.ToString("yyyyMMddHHmmss"));
                    break;
                case ValueType:
                    key.Append(param);
                    break;
                default:
                    key.Append(JsonSerializer.Serialize(param));
                    break;
            }
        }
        
        return key.ToString();
    }
}