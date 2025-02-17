using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcBookInfoServer.Resilience;

public class CircuitBreaker(int Threshold)
{
    private readonly object _lockObject = new object();
    private int _failureCount;
    private bool _open;

    public T Execute<T>(Func<T> func)
    {
        if (_open)
        {
            throw new RpcException(new Status(StatusCode.Aborted, $"Circuit is open"));
        }
        try
        {
            return func();
        }
        catch (RpcException ex)
        {
            lock (_lockObject)
            {
                _failureCount++;

                if (_failureCount >= Threshold)
                {
                    _open = true;

                    Task.Run(() =>
                    {
                        Thread.Sleep(5000);

                        lock (_lockObject)
                        {
                            _open = false;
                            _failureCount = 0;
                        }
                    });
                }
            }
            throw;
        }
    }
}
