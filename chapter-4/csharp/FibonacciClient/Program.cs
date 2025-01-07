using Grpc.Core;
using Grpc.Net.Client;
using Proto;
using System;
using System.Diagnostics;
using System.Threading;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new FibonacciService.FibonacciServiceClient(channel);

var replySync = await client.SyncFibonacciAsync(new FibonacciRequest { Number = 40 });

Console.WriteLine($"Synchronous Fibonacci sequence result for {40}: {replySync.FibonacciNumbers}, and time taken was: {replySync.TimeTaken} ticks");

var repliesAsync = client.AsyncFibonacci(new FibonacciRequest { Number = 40 });

//var cancellationToken = new CancellationToken();
//while (await replyAsync.ResponseStream.MoveNext(cancellationToken))
//{
//  Console.WriteLine($"Response form fibonacci stream: sequence({replyAsync.ResponseStream.Current.Sequence}), fibonacci number({replyAsync.ResponseStream.Current.FibonacciNumber})");
//}

try
{
  using var tokenSource = new CancellationTokenSource();

  var timeoutWatch = new Stopwatch();
  timeoutWatch.Restart();

  await foreach (var reply in repliesAsync.ResponseStream.ReadAllAsync(tokenSource.Token))
  {
    Console.WriteLine($"Response form fibonacci stream: sequence({repliesAsync.ResponseStream.Current.Sequence}), fibonacci number({repliesAsync.ResponseStream.Current.FibonacciNumber})");

    // Info CSharp-Fork: this is not in the book.
    if (timeoutWatch.Elapsed.Seconds > 10)
    {
      await tokenSource.CancelAsync();
    }
  }
}
catch (RpcException e) when (e.Status.StatusCode == StatusCode.Cancelled)
{
  Console.WriteLine("Streaming was cancelled from the client!");
}

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
