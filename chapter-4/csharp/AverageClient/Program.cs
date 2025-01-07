using Grpc.Net.Client;
using Proto;
using System;
using System.Collections.Generic;
using System.Threading;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new AverageService.AverageServiceClient(channel);

string arg = "4,5,1,10,40,50,2,6,0,8";

var requests = new List<AverageRequest>();

foreach (var strNumber in arg.Split(','))
{
  requests.Add(new AverageRequest { Number = int.Parse(strNumber) });
}

var stream = client.FindAverage();

foreach (var req in requests)
{
  await stream.RequestStream.WriteAsync(req);

  Thread.Sleep(1000);
}

await stream.RequestStream.CompleteAsync();

var res = await stream.ResponseAsync;

Console.WriteLine($"FindAverage response: {res.Average}");

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
