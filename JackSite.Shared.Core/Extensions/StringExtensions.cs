

namespace JackSite.Shared.Core.Extensions;

/// <summary>
/// 字符串扩展方法
/// </summary>
public static class StringExtensions
{
    #region 基本操作

    /// <summary>
    /// 判断字符串是否为空或空白
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 判断字符串是否不为空且不为空白
    /// </summary>
    public static bool IsNotNullOrWhiteSpace(this string? str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 安全地获取字符串，如果为 null 则返回空字符串
    /// </summary>
    public static string SafeString(this string? str)
    {
        return str ?? string.Empty;
    }

    /// <summary>
    /// 安全地获取字符串，如果为 null 则返回指定的默认值
    /// </summary>
    public static string SafeString(this string? str, string defaultValue)
    {
        return str ?? defaultValue;
    }

    /// <summary>
    /// 截取字符串，超出长度部分用省略号代替
    /// </summary>
    public static string Truncate(this string str, int maxLength, string suffix = "...")
    {
        return str.Length <= maxLength
            ? str
            : str[..maxLength] + suffix;
    }

    #endregion

    #region 转换操作

    /// <summary>
    /// 转换为 int，转换失败则返回默认值
    /// </summary>
    public static int ToInt(this string? str, int defaultValue = 0)
    {
        return int.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 转换为 long，转换失败则返回默认值
    /// </summary>
    public static long ToLong(this string? str, long defaultValue = 0)
    {
        return long.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 转换为 decimal，转换失败则返回默认值
    /// </summary>
    public static decimal ToDecimal(this string? str, decimal defaultValue = 0)
    {
        return decimal.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 转换为 double，转换失败则返回默认值
    /// </summary>
    public static double ToDouble(this string? str, double defaultValue = 0)
    {
        return double.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 转换为 bool，转换失败则返回默认值
    /// </summary>
    public static bool ToBool(this string? str, bool defaultValue = false)
    {
        if (string.IsNullOrWhiteSpace(str))
            return defaultValue;

        return str.ToLower() switch
        {
            "true" or "1" or "yes" or "y" or "on" => true,
            "false" or "0" or "no" or "n" or "off" => false,
            _ => bool.TryParse(str, out var result) ? result : defaultValue
        };
    }

    /// <summary>
    /// 转换为 DateTime，转换失败则返回默认值
    /// </summary>
    public static DateTime ToDateTime(this string? str, DateTime defaultValue = default)
    {
        return DateTime.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 转换为 Guid，转换失败则返回默认值
    /// </summary>
    public static Guid ToGuid(this string? str, Guid defaultValue = default)
    {
        return Guid.TryParse(str, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 转换为 Enum，转换失败则返回默认值
    /// </summary>
    public static T ToEnum<T>(this string? str, T defaultValue = default) where T : struct, Enum
    {
        return Enum.TryParse<T>(str, true, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// 转换为 JSON 对象
    /// </summary>
    public static T? ToObject<T>(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// 转换为 Base64 字符串
    /// </summary>
    public static string ToBase64(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 从 Base64 字符串转换
    /// </summary>
    public static string FromBase64(this string base64)
    {
        if (string.IsNullOrEmpty(base64))
            return string.Empty;

        try
        {
            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    #endregion

    #region 格式化操作

    /// <summary>
    /// 格式化字符串，类似 string.Format
    /// </summary>
    public static string FormatWith(this string format, params object[] args)
    {
        return string.Format(format, args);
    }

    /// <summary>
    /// 转换为驼峰命名
    /// </summary>
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        if (str.Length == 1)
            return str.ToLower();

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 转换为帕斯卡命名
    /// </summary>
    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        if (str.Length == 1)
            return str.ToUpper();

        return char.ToUpperInvariant(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// 转换为蛇形命名
    /// </summary>
    public static string ToSnakeCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return Regex.Replace(
            str,
            @"([a-z0-9])([A-Z])",
            "$1_$2").ToLower();
    }

    /// <summary>
    /// 转换为短横线命名
    /// </summary>
    public static string ToKebabCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return Regex.Replace(
            str,
            @"([a-z0-9])([A-Z])",
            "$1-$2").ToLower();
    }

    #endregion

    #region 验证操作

    /// <summary>
    /// 判断字符串是否是有效的电子邮件地址
    /// </summary>
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // 使用正则表达式验证邮箱格式
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 判断字符串是否是有效的 URL
    /// </summary>
    public static bool IsValidUrl(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    /// <summary>
    /// 判断字符串是否是有效的手机号码（中国大陆）
    /// </summary>
    public static bool IsValidMobilePhone(this string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        return Regex.IsMatch(phone, @"^1[3-9]\d{9}$");
    }

    /// <summary>
    /// 判断字符串是否是有效的身份证号码（中国大陆）
    /// </summary>
    public static bool IsValidIdCard(this string idCard)
    {
        if (string.IsNullOrWhiteSpace(idCard))
            return false;

        // 15 位或 18 位
        if (idCard.Length != 15 && idCard.Length != 18)
            return false;

        // 15 位全是数字
        if (idCard.Length == 15)
            return Regex.IsMatch(idCard, @"^\d{15}$");

        // 18 位，前 17 位是数字，最后一位可能是数字或 X
        return Regex.IsMatch(idCard, @"^\d{17}[\dX]$");
    }

    /// <summary>
    /// 判断字符串是否包含中文字符
    /// </summary>
    public static bool ContainsChinese(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
    }

    /// <summary>
    /// 判断字符串是否全是中文字符
    /// </summary>
    public static bool IsAllChinese(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        return Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$");
    }

    #endregion

    #region 加密操作

    /// <summary>
    /// 计算 MD5 哈希
    /// </summary>
    public static string ToMd5(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        using var md5 = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(str);
        var hashBytes = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    /// <summary>
    /// 计算 SHA256 哈希
    /// </summary>
    public static string ToSha256(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        using var sha256 = SHA256.Create();
        var inputBytes = Encoding.UTF8.GetBytes(str);
        var hashBytes = sha256.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    #endregion

    #region 其他操作

    /// <summary>
    /// 反转字符串
    /// </summary>
    public static string Reverse(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        var charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    /// <summary>
    /// 获取字符串中的数字
    /// </summary>
    public static string GetNumbers(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return new string(str.Where(char.IsDigit).ToArray());
    }

    /// <summary>
    /// 获取字符串中的字母
    /// </summary>
    public static string GetLetters(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return new string(str.Where(char.IsLetter).ToArray());
    }

    /// <summary>
    /// 移除字符串中的 HTML 标签
    /// </summary>
    public static string RemoveHtmlTags(this string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        return Regex.Replace(html, @"<[^>]+>", string.Empty);
    }

    /// <summary>
    /// 将字符串转换为标题格式（每个单词首字母大写）
    /// </summary>
    public static string ToTitleCase(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }

    /// <summary>
    /// 将字符串分割为列表
    /// </summary>
    public static List<string> SplitToList(this string str, char separator)
    {
        if (string.IsNullOrEmpty(str))
            return new List<string>();

        return str.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    /// <summary>
    /// 将字符串分割为列表
    /// </summary>
    public static List<string> SplitToList(this string str, string separator)
    {
        if (string.IsNullOrEmpty(str))
            return new List<string>();

        return str.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    /// <summary>
    /// 将字符串中的换行符替换为 HTML 的 <br/> 标签
    /// </summary>
    public static string NewLineToBr(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return str.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
    }

    /// <summary>
    /// 将字符串中的 HTML 的 <br/> 标签替换为换行符
    /// </summary>
    public static string BrToNewLine(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return Regex.Replace(str, @"<br\s*\/?>", Environment.NewLine);
    }

    /// <summary>
    /// 将字符串转换为 URL 友好的 Slug
    /// </summary>
    public static string ToSlug(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        // 转换为小写
        str = str.ToLowerInvariant();

        // 移除重音符号
        var normalizedString = str.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        str = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        // 替换非字母数字字符为连字符
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        // 替换多个空格为单个连字符
        str = Regex.Replace(str, @"\s+", "-");
        // 替换多个连字符为单个连字符
        str = Regex.Replace(str, @"-+", "-");
        // 移除开头和结尾的连字符
        str = str.Trim('-');

        return str;
    }

    #endregion
}