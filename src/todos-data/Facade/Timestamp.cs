using System;

namespace todos_data.Facade
{
    public interface ITimestamp
    {
        long TimestampInMiliseconds { get; set; }
    }

    public class Timestamp : ITimestamp
    {
        public long TimestampInMiliseconds { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}