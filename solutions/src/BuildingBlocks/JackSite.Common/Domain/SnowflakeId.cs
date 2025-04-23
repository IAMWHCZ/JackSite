namespace JackSite.Common.Domain
{
    public readonly struct SnowflakeId : IEquatable<SnowflakeId>, IComparable<SnowflakeId>
    {
        private static readonly IdWorker DefaultWorker = new(1, 1);
        private readonly long _value;

        public SnowflakeId()
        {
            _value = DefaultWorker.NextId();
        }

        private SnowflakeId(long value)
        {
            _value = value;
        }

        public static SnowflakeId NewId() => new(DefaultWorker.NextId());

        public static SnowflakeId Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (long.TryParse(value, out var longValue))
                return new SnowflakeId(longValue);

            throw new FormatException("Invalid SnowflakeId format");
        }

        public static bool TryParse(string? value, out SnowflakeId result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (!long.TryParse(value, out var longValue)) return false;
        
            result = new SnowflakeId(longValue);
            return true;
        }

        public static implicit operator long(SnowflakeId id) => id._value;
        public static explicit operator SnowflakeId(long value) => new(value);

        public override string ToString() => _value.ToString();

        public bool Equals(SnowflakeId other) => _value == other._value;
        public override bool Equals(object? obj) => obj is SnowflakeId other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();

        public int CompareTo(SnowflakeId other) => _value.CompareTo(other._value);

        public static bool operator ==(SnowflakeId left, SnowflakeId right) => left.Equals(right);
        public static bool operator !=(SnowflakeId left, SnowflakeId right) => !left.Equals(right);
        public static bool operator <(SnowflakeId left, SnowflakeId right) => left.CompareTo(right) < 0;
        public static bool operator <=(SnowflakeId left, SnowflakeId right) => left.CompareTo(right) <= 0;
        public static bool operator >(SnowflakeId left, SnowflakeId right) => left.CompareTo(right) > 0;
        public static bool operator >=(SnowflakeId left, SnowflakeId right) => left.CompareTo(right) >= 0;

        public DateTime GetTimestamp()
        {
            const long twepoch = 1288834974657L;
            var timestamp = (_value >> 22) + twepoch;
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
        }

        public (int DatacenterId, int WorkerId, int Sequence) GetComponents()
        {
            var datacenterId = (int)((_value >> 17) & 0x1F);
            var workerId = (int)((_value >> 12) & 0x1F);
            var sequence = (int)(_value & 0xFFF);
            return (datacenterId, workerId, sequence);
        }
    }

    // IdWorker class implementation (as shown in previous response)
    internal class IdWorker
    {
        private const long Twepoch = 1288834974657L;
        private const int WorkerIdBits = 5;
        private const int DatacenterIdBits = 5;
        private const int SequenceBits = 12;
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

        private readonly long _workerId;
        private readonly long _datacenterId;
        private long _sequence;
        private long _lastTimestamp = -1L;
        private static readonly object LockObject = new();

        public IdWorker(long workerId, long datacenterId, long sequence = 0L)
        {
            if (workerId > MaxWorkerId || workerId < 0)
                throw new ArgumentException($"worker Id must be greater than 0 and less than {MaxWorkerId}");

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
                throw new ArgumentException($"datacenter Id must be greater than 0 and less than {MaxDatacenterId}");

            _workerId = workerId;
            _datacenterId = datacenterId;
            _sequence = sequence;
        }

        public long NextId()
        {
            lock (LockObject)
            {
                var timestamp = TimeGen();

                if (timestamp < _lastTimestamp)
                    throw new Exception($"Clock moved backwards. Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");

                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & SequenceMask;
                    if (_sequence == 0)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;

                return ((timestamp - Twepoch) << TimestampLeftShift) |
                       (_datacenterId << DatacenterIdShift) |
                       (_workerId << WorkerIdShift) |
                       _sequence;
            }
        }

        private static long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        private static long TimeGen()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }

}