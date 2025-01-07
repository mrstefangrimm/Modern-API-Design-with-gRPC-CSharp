using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Proto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new MaxService.MaxServiceClient(channel);

string arg = "4,5,1,10,40,50,2,6,0,8,100";

var requests = new List<MaxRequest>();

foreach (var strNumber in arg.Split(','))
{
  requests.Add(new MaxRequest { Number = int.Parse(strNumber) });
}

var stream = client.FindMax();

var writeTask = Task.Factory.StartNew(() =>
{
  foreach (var req in requests)
  {
    stream.RequestStream.WriteAsync(req);

    Thread.Sleep(1000);
  }

  stream.RequestStream.CompleteAsync();
});

var readTask = Task.Factory.StartNew(() =>
{
  var cancellationToken = new CancellationToken();
  while (stream.ResponseStream.MoveNext(cancellationToken).Result)
  {
    Console.WriteLine($"Response received: {stream.ResponseStream.Current.Max}");
  }
});

await Task.WhenAll(writeTask, readTask);

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
