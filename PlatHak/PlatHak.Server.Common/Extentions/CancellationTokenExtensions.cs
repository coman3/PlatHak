using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlatHak.Server.Common.Extentions
{
    internal static class CancellationTokenExtensions
    {
        public static bool WaitCancellationRequested(this CancellationToken token, TimeSpan timeout)
        {
            return token.WaitHandle.WaitOne(timeout);
        }
    }
    internal static class Repeat
    {
        public static Task Interval(TimeSpan pollInterval, Action action, CancellationToken token)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        if (token.WaitCancellationRequested(pollInterval))
                            break;

                        action();
                    }
                }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}