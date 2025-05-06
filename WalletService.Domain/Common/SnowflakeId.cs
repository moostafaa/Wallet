using System;

namespace WalletService.Domain.Common
{
    public class SnowflakeId
    {
        private const long EPOCH = 1609459200000L; // 2021-01-01 00:00:00 UTC
        private const int SEQUENCE_BITS = 12;
        private const int WORKER_ID_BITS = 5;
        private const int DATACENTER_ID_BITS = 5;
        
        private const int MAX_WORKER_ID = -1 ^ (-1 << WORKER_ID_BITS);
        private const int MAX_DATACENTER_ID = -1 ^ (-1 << DATACENTER_ID_BITS);
        
        private const int WORKER_ID_SHIFT = SEQUENCE_BITS;
        private const int DATACENTER_ID_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS;
        private const int TIMESTAMP_LEFT_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS + DATACENTER_ID_BITS;
        
        private const long SEQUENCE_MASK = -1L ^ (-1L << SEQUENCE_BITS);
        
        private readonly int _workerId;
        private readonly int _datacenterId;
        private long _sequence = 0L;
        private long _lastTimestamp = -1L;
        
        private static readonly object _lock = new object();
        
        public SnowflakeId(int workerId, int datacenterId)
        {
            if (workerId > MAX_WORKER_ID || workerId < 0)
                throw new ArgumentException($"Worker ID can't be greater than {MAX_WORKER_ID} or less than 0");
                
            if (datacenterId > MAX_DATACENTER_ID || datacenterId < 0)
                throw new ArgumentException($"Datacenter ID can't be greater than {MAX_DATACENTER_ID} or less than 0");
                
            _workerId = workerId;
            _datacenterId = datacenterId;
        }
        
        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();
                
                if (timestamp < _lastTimestamp)
                    throw new InvalidOperationException("Clock moved backwards");
                    
                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & SEQUENCE_MASK;
                    if (_sequence == 0)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0L;
                }
                
                _lastTimestamp = timestamp;
                
                return ((timestamp - EPOCH) << TIMESTAMP_LEFT_SHIFT) |
                       (_datacenterId << DATACENTER_ID_SHIFT) |
                       (_workerId << WORKER_ID_SHIFT) |
                       _sequence;
            }
        }
        
        private long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }
        
        private long TimeGen()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}