using System;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace todos_tests.Utility
{
    public static class Exceptions
    {
        public static void HandleExceptions<T>(Action actionToRun, Action<T> errorToThrow) where T : Exception
        {
            try
            {
                actionToRun();

                throw new XunitException("should not continue on");
            }
            catch (T ex)
            {
                errorToThrow(ex);
            }
        }

        public static async Task HandleExceptionsAsync<T>(Func<Task> actionToRun, Action<T> errorToThrow) where T : Exception
        {
            try
            {
                await actionToRun();

                throw new XunitException("should not continue on");
            }
            catch (T ex)
            {
                errorToThrow(ex);
            }
        }
    }
}