﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlatHak.Common.Network.Sockets.Extensions
{
    // I don't remember where this is taken/adapted from. 
    public static class AsyncExtensions
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout, CancellationTokenSource cancellationTokenSource = null)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeout)))
                return await task;
            else
            {
                if (cancellationTokenSource != null)
                    cancellationTokenSource.Cancel();

                throw new TimeoutException();
            }
        }

    }
}
