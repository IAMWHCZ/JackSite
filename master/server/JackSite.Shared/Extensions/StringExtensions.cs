using System.Text.RegularExpressions;

namespace JackSite.Shared.Extensions;

/// <summary>
/// 提供字符串操作的扩展方法
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 检查字符串是否为空或仅包含空白字符
    /// </summary>
    /// <param name="str">要检查的字符串</param>
    /// <returns>如果字符串为 null、空或仅包含空白字符，则返回 true；否则返回 false</returns>
    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 检查字符串是否为空
    /// </summary>
    /// <param name="str">要检查的字符串</param>
    /// <returns>如果字符串为 null 或空，则返回 true；否则返回 false</returns>
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 如果字符串为 null，则返回空字符串
    /// </summary>
    /// <param name="str">要检查的字符串</param>
    /// <returns>如果字符串为 null，则返回空字符串；否则返回原字符串</returns>
    public static string EmptyIfNull(this string? str)
    {
        return str ?? string.Empty;
    }

    /// <summary>
    /// 截取字符串到指定长度，如果超出长度则添加省略号
    /// </summary>
    /// <param name="str">要截取的字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <param name="suffix">超出长度时添加的后缀，默认为"..."</param>
    /// <returns>截取后的字符串</returns>
    public static string Truncate(this string str, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(str) || str.Length <= maxLength)
        {
            return str;
        }

        return str[..maxLength] + suffix;
    }

    /// <summary>
    /// 将字符串转换为驼峰命名法（首字母小写）
    /// </summary>
    /// <param name="str">要转换的字符串</param>
    /// <returns>转换后的驼峰命名法字符串</returns>
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str.Length == 1)
        {
            return str.ToLowerInvariant();
        }

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 将字符串转换为帕斯卡命名法（首字母大写）
    /// </summary>
    /// <param name="str">要转换的字符串</param>
    /// <returns>转换后的帕斯卡命名法字符串</returns>
    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str.Length == 1)
        {
            return str.ToUpperInvariant();
        }

        return char.ToUpperInvariant(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 检查字符串是否包含指定的子字符串（忽略大小写）
    /// </summary>
    /// <param name="str">要检查的字符串</param>
    /// <param name="value">要查找的子字符串</param>
    /// <returns>如果包含指定的子字符串，则返回 true；否则返回 false</returns>
    public static bool ContainsIgnoreCase(this string str, string value)
    {
        return str.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 将字符串转换为指定类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="str">要转换的字符串</param>
    /// <returns>转换后的值</returns>
    public static T? To<T>(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return default;
        }

        try
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }
        catch
        {
            return default;
        }
    }
    /// <summary>
    /// 使用正则实现命名小写蛇形
    /// </summary>
    /// <param name="input">字符串</param>
    /// <returns>小写蛇形字符串</returns>
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static string StripHtml(this string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return string.Empty;
        }

        // 简单替换HTML标签
        var result = Regex.Replace(html, "<[^>]*>", string.Empty);
        // 替换HTML实体
        result = Regex.Replace(result, "&nbsp;", " ");
        result = Regex.Replace(result, "&lt;", "<");
        result = Regex.Replace(result, "&gt;", ">");
        result = Regex.Replace(result, "&amp;", "&");
        result = Regex.Replace(result, "&quot;", "\"");
        // 去除多余空白
        result = Regex.Replace(result, @"\s+", " ");
    
        return result.Trim();
    }
    
    public static string ToMimeType(this string extension)
    {
        return extension.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".zip" => "application/zip",
            ".rar" => "application/x-rar-compressed",
            ".7z" => "application/x-7z-compressed",
            _ => "application/octet-stream" // 默认二进制流
        };
    }
}