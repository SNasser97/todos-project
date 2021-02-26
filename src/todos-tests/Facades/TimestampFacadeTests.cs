using System;
using Moq;
using todos_data.Facade;
using todos_tests.Utility;
using Xunit;

namespace todos_tests.Facades
{
    public class TimestampFacadeTests
    {
        [Fact]
        public void TimestampFacadeTakesNullTimestampAndExpectsToThrowArgumentNullException()
        {
            Exceptions.HandleExceptions<ArgumentNullException>(() =>
                new TimestampFacade(null),
                (ex) => Assert.Equal("timestamp", ex.ParamName)
            );
        }
        [Fact]
        public void TimestampFacadeTakesTimeStampAndCallsGetTimestampInMilisecondsExpectsToReturnALongValue()
        {
            //Given I have a mock timestamp
            var timestamp = new Timestamp();

            //And I have an expected timestamp
            long expectedTimestamp = timestamp.TimestampInMiliseconds;
            
            //And I have a timestampFacade
            var timestampFacade = new TimestampFacade(timestamp);

            //When I call GetTimestampInMiliseconds
            long actualTimestamp = timestampFacade.GetTimestampInMilliseconds();
            Assert.True(actualTimestamp > 0);
            
            //Then I expect a long time
            Assert.Equal(expectedTimestamp, actualTimestamp);
        }
    }
}