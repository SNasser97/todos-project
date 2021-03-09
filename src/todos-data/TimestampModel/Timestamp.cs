using System;

namespace todos_data.TimestampModel
{
    public interface ITimestamp
    {
        long TimestampInMiliseconds { get; }
    }

    public class Timestamp : ITimestamp
    {
        public long TimestampInMiliseconds { get => DateTimeOffset.Now.ToUnixTimeMilliseconds(); }
    }
}