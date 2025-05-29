namespace JackSite.Authentication.Common;

public class SnowflakeIdGenerator
{
    public static readonly DateTime Epoch = new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private readonly Lock _lock = new();
    private readonly long _machineId;
    private readonly long _datacenterId;
    private long _lastTimestamp = -1L;
    private long _sequence;

    private const int MachineIdBits = 5;
    private const int DatacenterIdBits = 5;
    private const int SequenceBits = 12;
    
    private const long MaxMachineId = -1L ^ (-1L << MachineIdBits);
    private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
    private const long MaxSequence = -1L ^ (-1L << SequenceBits);
    
    private const int MachineIdShift = SequenceBits;
    private const int DatacenterIdShift = SequenceBits + MachineIdBits;
    private const int TimestampLeftShift = SequenceBits + MachineIdBits + DatacenterIdBits;

    public SnowflakeIdGenerator(long machineId, long datacenterId)
    {
        if (machineId is > MaxMachineId or < 0)
        {
            throw new ArgumentException($"Machine ID must be between 0 and {MaxMachineId}");
        }

        if (datacenterId is > MaxDatacenterId or < 0)
        {
            throw new ArgumentException($"Datacenter ID must be between 0 and {MaxDatacenterId}");
        }

        _machineId = machineId;
        _datacenterId = datacenterId;
    }

    public long NextId()
    {
        lock (_lock)
        {
            var timestamp = GetCurrentTimestamp();

            if (timestamp < _lastTimestamp)
            {
                throw new InvalidOperationException("Clock moved backwards, refusing to generate ID");
            }

            if (_lastTimestamp == timestamp)
            {
                _sequence = (_sequence + 1) & MaxSequence;
                if (_sequence == 0)
                {
                    timestamp = WaitNextMillis(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;

            return ((timestamp) << TimestampLeftShift) |
                   (_datacenterId << DatacenterIdShift) |
                   (_machineId << MachineIdShift) |
                   _sequence;
        }
    }

    private long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
    }

    private long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }
}