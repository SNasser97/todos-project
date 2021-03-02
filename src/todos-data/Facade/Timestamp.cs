using System;

namespace todos_data.Facade
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