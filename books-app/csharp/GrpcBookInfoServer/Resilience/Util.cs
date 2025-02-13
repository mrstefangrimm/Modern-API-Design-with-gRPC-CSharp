using Grpc.Core;
using System;
using System.Threading;

namespace GrpcBookInfoServer.Resilience;

public static class Util
{
    public static T WithRetry<T>(this Func<T> retryFunc, int maxAttempts, TimeSpan retryInterval)
    {
        for (var i = 0; i < maxAttempts; i++)
        {
            try
            {
                return retryFunc();
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"Failed with: {ex.StatusCode} at attempt: {i}");
            }
            Thread.Sleep(retryInterval);
        }

        throw new RpcException(new Status(StatusCode.Aborted, $"{maxAttempts} attempts failed."));
    }
}
