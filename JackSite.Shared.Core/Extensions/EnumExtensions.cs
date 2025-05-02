namespace JackSite.Shared.Core.Extensions;

/// <summary>
/// 枚举扩展方法
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 获取枚举值的描述
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>描述文本，如果没有描述特性则返回枚举名称</returns>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// 获取枚举值的显示名称
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>显示名称，如果没有显示特性则返回描述或枚举名称</returns>
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
            return displayAttribute.Name ?? value.ToString();

        var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// 获取枚举类型的所有值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举值列表</returns>
    public static IEnumerable<T> GetValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    /// <summary>
    /// 获取枚举类型的所有名称
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举名称列表</returns>
    public static IEnumerable<string> GetNames<T>() where T : Enum
    {
        return Enum.GetNames(typeof(T));
    }

    /// <summary>
    /// 获取枚举值的名称
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>枚举名称</returns>
    public static string GetName(this Enum value)
    {
        return Enum.GetName(value.GetType(), value) ?? value.ToString();
    }

    /// <summary>
    /// 将字符串转换为枚举值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">字符串值</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>枚举值，转换失败则返回默认值</returns>
    public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return default;

        return Enum.TryParse<T>(value, ignoreCase, out var result) ? result : default;
    }

    /// <summary>
    /// 将字符串转换为枚举值，转换失败则返回指定的默认值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">字符串值</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>枚举值</returns>
    public static T ToEnum<T>(this string value, T defaultValue, bool ignoreCase = true) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        return Enum.TryParse<T>(value, ignoreCase, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 将整数转换为枚举值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">整数值</param>
    /// <returns>枚举值，转换失败则返回默认值</returns>
    public static T ToEnum<T>(this int value) where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value))
            return (T)Enum.ToObject(typeof(T), value);
        
        return default;
    }

    /// <summary>
    /// 将整数转换为枚举值，转换失败则返回指定的默认值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">整数值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>枚举值</returns>
    public static T ToEnum<T>(this int value, T defaultValue) where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value))
            return (T)Enum.ToObject(typeof(T), value);
        
        return defaultValue;
    }

    /// <summary>
    /// 判断枚举值是否定义在枚举类型中
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">整数值</param>
    /// <returns>是否定义</returns>
    public static bool IsDefined<T>(this int value) where T : Enum
    {
        return Enum.IsDefined(typeof(T), value);
    }

    /// <summary>
    /// 判断字符串是否可以转换为指定的枚举类型
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="value">字符串值</param>
    /// <param name="ignoreCase">是否忽略大小写</param>
    /// <returns>是否可以转换</returns>
    public static bool IsEnumDefined<T>(this string value, bool ignoreCase = true) where T : Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Enum.TryParse(typeof(T), value, ignoreCase, out _);
    }

    /// <summary>
    /// 获取枚举类型的键值对字典
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举键值对字典</returns>
    public static Dictionary<int, string> ToDictionary<T>() where T : Enum
    {
        return GetValues<T>().ToDictionary(v => Convert.ToInt32(v), v => v.ToString());
    }

    /// <summary>
    /// 获取枚举类型的描述字典
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举描述字典</returns>
    public static Dictionary<int, string> ToDescriptionDictionary<T>() where T : Enum
    {
        return GetValues<T>().ToDictionary(v => Convert.ToInt32(v), v => v.GetDescription());
    }

    /// <summary>
    /// 获取枚举类型的显示名称字典
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举显示名称字典</returns>
    public static Dictionary<int, string> ToDisplayNameDictionary<T>() where T : Enum
    {
        return GetValues<T>().ToDictionary(v => Convert.ToInt32(v), v => v.GetDisplayName());
    }

    /// <summary>
    /// 检查枚举值是否包含指定标志
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <param name="flag">标志</param>
    /// <returns>是否包含</returns>
    public static bool HasFlag(this Enum value, Enum flag)
    {
        return value.HasFlag(flag);
    }

    /// <summary>
    /// 将枚举值转换为列表
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>枚举值列表</returns>
    public static List<T> ToList<T>() where T : Enum
    {
        return GetValues<T>().ToList();
    }

    /// <summary>
    /// 获取枚举值的整数值
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>整数值</returns>
    public static int ToInt(this Enum value)
    {
        return Convert.ToInt32(value);
    }

    /// <summary>
    /// 获取枚举值的长整数值
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <returns>长整数值</returns>
    public static long ToLong(this Enum value)
    {
        return Convert.ToInt64(value);
    }
}
