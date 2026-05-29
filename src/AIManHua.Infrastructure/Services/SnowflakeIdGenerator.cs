namespace AIManHua.Infrastructure.Services;

public sealed class SnowflakeIdGenerator
{
    private const long Epoch = 1700000000000L; // 2023-11-15 00:00:00 UTC
    private const int WorkerIdBits = 10;
    private const int SequenceBits = 12;
    private const long MaxWorkerId = (1L << WorkerIdBits) - 1;
    private const long SequenceMask = (1L << SequenceBits) - 1;
    private const int TimestampShift = WorkerIdBits + SequenceBits;
    private const int WorkerIdShift = SequenceBits;

    private long _lastTimestamp = -1L;
    private long _sequence;

    private static readonly DateTimeOffset EpochDateTime =
        DateTimeOffset.FromUnixTimeMilliseconds(Epoch);

    public long WorkerId { get; }

    public SnowflakeIdGenerator(long workerId)
    {
        if (workerId < 0 || workerId > MaxWorkerId)
            throw new ArgumentException($"WorkerId must be 0~{MaxWorkerId}", nameof(workerId));
        WorkerId = workerId;
    }

    public long NextId()
    {
        var timestamp = CurrentTimestamp();
        if (timestamp < _lastTimestamp)
            throw new InvalidOperationException("Clock moved backwards, refusing to generate id");

        if (timestamp == _lastTimestamp)
        {
            _sequence = (_sequence + 1) & SequenceMask;
            if (_sequence == 0)
                timestamp = WaitNextMillis(_lastTimestamp);
        }
        else
        {
            _sequence = 0;
        }

        _lastTimestamp = timestamp;
        return ((timestamp - Epoch) << TimestampShift)
               | (WorkerId << WorkerIdShift)
               | _sequence;
    }

    public static DateTimeOffset ExtractTimestamp(long id)
    {
        var timestamp = (id >> TimestampShift) + Epoch;
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
    }

    private static long CurrentTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    private static long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = CurrentTimestamp();
        while (timestamp <= lastTimestamp)
            timestamp = CurrentTimestamp();
        return timestamp;
    }
}
