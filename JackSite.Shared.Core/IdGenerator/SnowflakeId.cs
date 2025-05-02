namespace JackSite.Shared.Core.IdGenerator;

/// <summary>
/// 雪花ID结构体，提供类似Guid的使用体验
/// </summary>
public readonly struct SnowflakeId : IEquatable<SnowflakeId>, IComparable<SnowflakeId>, IComparable
{
    private readonly long _value;

    /// <summary>
    /// 空雪花ID
    /// </summary>
    public static readonly SnowflakeId Empty = new(0);

    /// <summary>
    /// 初始化雪花ID
    /// </summary>
    /// <param name="value">ID值</param>
    public SnowflakeId(long value)
    {
        _value = value;
    }

    /// <summary>
    /// 生成新的雪花ID
    /// </summary>
    /// <returns>雪花ID</returns>
    public static SnowflakeId NewId()
    {
        return new SnowflakeId(IdGeneratorExtensions.NewId());
    }

    /// <summary>
    /// 转换为长整型
    /// </summary>
    /// <returns>长整型值</returns>
    public long ToInt64()
    {
        return _value;
    }

    public bool Equals(SnowflakeId other)
    {
        return _value == other._value;
    }

    public int CompareTo(SnowflakeId other)
    {
        return _value.CompareTo(other._value);
    }

    public int CompareTo(object? obj)
    {
        if (obj is SnowflakeId other)
        {
            return CompareTo(other);
        }
        
        throw new ArgumentException("Object must be of type SnowflakeId", nameof(obj));
    }

    public override bool Equals(object? obj)
    {
        return obj is SnowflakeId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static bool operator ==(SnowflakeId left, SnowflakeId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SnowflakeId left, SnowflakeId right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(SnowflakeId left, SnowflakeId right)
    {
        return left.CompareTo(right) >= 0;
    }

    public override string ToString()
    {
        return _value.ToString();
    }
}
