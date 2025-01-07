using Grpc.Core;
using Proto;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Server
{
  public class GrpcFibonacciService : FibonacciService.FibonacciServiceBase
  {
    public override Task<SyncFibonacciResponse> SyncFibonacci(FibonacciRequest request, ServerCallContext context)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Restart();
      var fibonacciNumbers = new Int32[request.Number];
      for (int i = 0; i < request.Number; i++)
      {
        fibonacciNumbers[i] = Fib(i);
      }
      stopwatch.Stop();

      var resp = new SyncFibonacciResponse {
        TimeTaken = stopwatch.ElapsedTicks.ToString()
      };
      resp.FibonacciNumbers.AddRange(fibonacciNumbers);

      return Task.FromResult(resp);
    }

    public override Task AsyncFibonacci(FibonacciRequest request, IServerStreamWriter<AsyncFibonacciResponse> responseStream, ServerCallContext context)
    {
      for (int i = 0; i < request.Number; i++)
      {
        var fibI = Fib(i);
        var resp = new AsyncFibonacciResponse {
          Sequence = i,
          FibonacciNumber = fibI
        };
        responseStream.WriteAsync(resp);
      }

      return Task.CompletedTask;
    }

    private static Int32 Fib(Int32 n)
    {
      if (n <= 0)
      {
        return 0;
      }
      else if (n == 1)
      {
        return 1;
      }
      else
      {
        return Fib(n - 1) + Fib(n - 2);
      }
    }
  }
}
