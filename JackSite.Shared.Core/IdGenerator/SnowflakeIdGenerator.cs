namespace JackSite.Shared.Core.IdGenerator;

/// <summary>
/// 雪花 ID 生成器
/// </summary>
public class SnowflakeIdGenerator
{
    private const long Twepoch = 1288834974657L; // 起始时间戳 (2010-11-04 01:42:54.657)
    
    private const int WorkerIdBits = 5; // 机器 ID 所占位数
    private const int DatacenterIdBits = 5; // 数据中心 ID 所占位数
    private const int SequenceBits = 12; // 序列号所占位数
    
    private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits); // 最大机器 ID
    private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits); // 最大数据中心 ID
    
    private const int WorkerIdShift = SequenceBits; // 机器 ID 左移位数
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits; // 数据中心 ID 左移位数
    private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits; // 时间戳左移位数
    
    private const long SequenceMask = -1L ^ (-1L << SequenceBits); // 序列号掩码
    
    private readonly long _workerId; // 机器 ID
    private readonly long _datacenterId; // 数据中心 ID
    private long _sequence = 0L; // 序列号
    private long _lastTimestamp = -1L; // 上次生成 ID 的时间戳
    
    private readonly Lock _lock = new();
    
    /// <summary>
    /// 初始化雪花 ID 生成器
    /// </summary>
    /// <param name="workerId">机器 ID</param>
    /// <param name="datacenterId">数据中心 ID</param>
    public SnowflakeIdGenerator(long workerId, long datacenterId)
    {
        if (workerId is > MaxWorkerId or < 0)
            throw new ArgumentException($"Worker ID 不能大于 {MaxWorkerId} 或小于 0");
            
        if (datacenterId is > MaxDatacenterId or < 0)
            throw new ArgumentException($"Datacenter ID 不能大于 {MaxDatacenterId} 或小于 0");
            
        _workerId = workerId;
        _datacenterId = datacenterId;
    }
    
    /// <summary>
    /// 生成下一个 ID
    /// </summary>
    /// <returns>雪花 ID</returns>
    public  long NextId()
    {
        lock (_lock)
        {
            var timestamp = TimeGen();
            
            // 如果当前时间小于上一次 ID 生成的时间戳，说明系统时钟回退过，抛出异常
            if (timestamp < _lastTimestamp)
                throw new InvalidOperationException($"时钟回退，拒绝生成 ID，回退时间: {_lastTimestamp - timestamp} 毫秒");
                
            // 如果是同一时间生成的，则进行序列号递增
            if (_lastTimestamp == timestamp)
            {
                _sequence = (_sequence + 1) & SequenceMask;
                // 序列号溢出，等待下一毫秒
                if (_sequence == 0)
                    timestamp = TilNextMillis(_lastTimestamp);
            }
            else
            {
                // 时间戳改变，序列号重置
                _sequence = 0L;
            }
            
            _lastTimestamp = timestamp;
            
            // 生成 ID
            return ((timestamp - Twepoch) << TimestampLeftShift) |
                   (_datacenterId << DatacenterIdShift) |
                   (_workerId << WorkerIdShift) |
                   _sequence;
        }
    }
    
    /// <summary>
    /// 等待下一毫秒
    /// </summary>
    /// <param name="lastTimestamp">上次生成 ID 的时间戳</param>
    /// <returns>下一毫秒的时间戳</returns>
    private static long TilNextMillis(long lastTimestamp)
    {
        var timestamp = TimeGen();
        while (timestamp <= lastTimestamp)
            timestamp = TimeGen();
        return timestamp;
    }
    
    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <returns>当前时间戳（毫秒）</returns>
    private static long TimeGen()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}