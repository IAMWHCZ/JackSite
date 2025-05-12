namespace JackSite.Shared.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举值的描述特性内容
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>描述特性内容，如果没有则返回枚举名称</returns>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();
        
        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute?.Description ?? value.ToString();
    }
    
    /// <summary>
    /// 将字符串转换为指定枚举类型的值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">要转换的字符串</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>转换后的枚举值</returns>
    public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct, Enum
    {
        return Enum.Parse<T>(value, ignoreCase);
    }
    
    /// <summary>
    /// 尝试将字符串转换为指定枚举类型的值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">要转换的字符串</param>
    /// <param name="result">转换结果</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>是否转换成功</returns>
    public static bool TryToEnum<T>(this string value, out T result, bool ignoreCase = true) where T : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase, out result);
    }
    
    /// <summary>
    /// 获取枚举类型的所有值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举值数组</returns>
    public static T[] GetValues<T>() where T : struct, Enum
    {
        return Enum.GetValues<T>();
    }
    
    /// <summary>
    /// 获取枚举类型的所有值及其描述的字典
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举值和描述的字典</returns>
    public static Dictionary<T, string> GetDescriptionDictionary<T>() where T : struct, Enum
    {
        return GetValues<T>().ToDictionary(v => v, v => v.GetDescription());
    }
    
    /// <summary>
    /// 获取枚举类型的所有名称
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举名称数组</returns>
    public static string[] GetNames<T>() where T : struct, Enum
    {
        return Enum.GetNames<T>();
    }
    
    /// <summary>
    /// 获取枚举类型的所有名称及其描述的字典
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举名称和描述的字典</returns>
    public static Dictionary<string, string> GetNameDescriptionDictionary<T>() where T : struct, Enum
    {
        return GetNames<T>().ToDictionary(name => name, name => ((T)Enum.Parse(typeof(T), name)).GetDescription());
    }
    
    /// <summary>
    /// 根据描述获取枚举值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="description">描述</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>枚举值，如果未找到则返回默认值</returns>
    public static T GetEnumFromDescription<T>(string description, bool ignoreCase = true) where T : struct, Enum
    {
        var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        
        foreach (var value in GetValues<T>())
        {
            if (string.Equals(value.GetDescription(), description, comparisonType))
            {
                return value;
            }
        }
        
        return default;
    }
    
    /// <summary>
    /// 尝试根据描述获取枚举值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="description">描述</param>
    /// <param name="result">转换结果</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>是否找到匹配的枚举值</returns>
    public static bool TryGetEnumFromDescription<T>(string description, out T result, bool ignoreCase = true) where T : struct, Enum
    {
        result = GetEnumFromDescription<T>(description, ignoreCase);
        return !EqualityComparer<T>.Default.Equals(result, default);
    }
    
    /// <summary>
    /// 检查枚举值是否定义在枚举类型中
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">要检查的值</param>
    /// <returns>是否定义在枚举类型中</returns>
    public static bool IsDefined<T>(this T value) where T : struct, Enum
    {
        return Enum.IsDefined(typeof(T), value);
    }
    
    /// <summary>
    /// 获取枚举值的底层数值
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>底层数值</returns>
    public static int GetUnderlyingValue(this Enum value)
    {
        return Convert.ToInt32(value);
    }
    
    /// <summary>
    /// 获取枚举类型的所有值及其底层数值的字典
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举值和底层数值的字典</returns>
    public static Dictionary<T, int> GetValueDictionary<T>() where T : struct, Enum
    {
        return GetValues<T>().ToDictionary(v => v, v => Convert.ToInt32(v));
    }
    
    /// <summary>
    /// 检查对象是否为枚举类型
    /// </summary>
    /// <param name="obj">要检查的对象</param>
    /// <returns>是否为枚举类型</returns>
    public static bool IsEnum(this object obj)
    {
        if (obj == null) return false;
        return obj.GetType().IsEnum;
    }
    
    /// <summary>
    /// 检查类型是否为枚举类型
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <returns>是否为枚举类型</returns>
    public static bool IsEnumType(this Type type)
    {
        if (type == null) return false;
        return type.IsEnum;
    }
    
    /// <summary>
    /// 检查对象是否为指定枚举类型的值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="obj">要检查的对象</param>
    /// <returns>是否为指定枚举类型的值</returns>
    public static bool IsEnumOfType<T>(this object obj) where T : struct, Enum
    {
        if (obj == null) return false;
        return obj.GetType() == typeof(T);
    }
    
    /// <summary>
    /// 检查整数值是否可以转换为指定枚举类型的有效值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">整数值</param>
    /// <returns>是否可以转换为有效的枚举值</returns>
    public static bool IsValidEnumValue<T>(this int value) where T : struct, Enum
    {
        return Enum.IsDefined(typeof(T), value);
    }
    
    /// <summary>
    /// 检查字符串是否可以转换为指定枚举类型的有效值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">字符串值</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>是否可以转换为有效的枚举值</returns>
    public static bool IsValidEnumName<T>(this string value, bool ignoreCase = true) where T : struct, Enum
    {
        if (string.IsNullOrEmpty(value)) return false;
        
        try
        {
            var result = Enum.Parse<T>(value, ignoreCase);
            return Enum.IsDefined(result);
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// 检查字符串是否匹配指定枚举类型的任何描述
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="description">描述字符串</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>是否匹配任何枚举值的描述</returns>
    public static bool IsValidEnumDescription<T>(this string description, bool ignoreCase = true) where T : struct, Enum
    {
        if (string.IsNullOrEmpty(description)) return false;
        
        var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        return GetValues<T>().Any(v => string.Equals(v.GetDescription(), description, comparisonType));
    }
    
    /// <summary>
    /// 将整数值安全地转换为枚举值，如果不是有效的枚举值则返回默认值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">整数值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>转换后的枚举值</returns>
    public static T ToEnumSafe<T>(this int value, T defaultValue = default) where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
        
        return defaultValue;
    }
}
