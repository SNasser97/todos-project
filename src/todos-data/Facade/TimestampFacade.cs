using System;

namespace todos_data.Facade
{
    public interface ITimestampFacade
    {
        long GetTimestampInMilliseconds();
    }

    public class TimestampFacade : ITimestampFacade
    {
        private readonly ITimestamp timestamp;

        public TimestampFacade() : this (new Timestamp())
        {
            
        }

        public TimestampFacade(ITimestamp timestamp)
        {
            this.timestamp = timestamp ?? throw new ArgumentNullException(nameof(timestamp));
        }

        public long GetTimestampInMilliseconds()
        {
            return this.timestamp.TimestampInMiliseconds;
        }
    }
}