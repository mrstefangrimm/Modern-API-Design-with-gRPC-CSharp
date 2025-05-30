using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Proto;
using System;
using System.IO;
using System.Threading.Tasks;

var builder = WebApplication.CreateSlimBuilder(args);

Console.WriteLine("Hello.");

var grpcAddress = builder.Configuration["GrpcGreetServerUrl"]!;
Console.WriteLine($"Expecting grpc greet server address: {grpcAddress}");

using var channel = GrpcChannel.ForAddress(grpcAddress);
var client = new GreetService.GreetServiceClient(channel);

var app = builder.Build();

var greetApi = app.MapGroup("/");

greetApi.MapPost("/greet", async Task<string> (HttpRequest request) =>
{
  Console.WriteLine($"MapPost, grpc address:{grpcAddress}");

  using var body = new StreamReader(request.Body);
  var data = await body.ReadToEndAsync();
  var greeter = JsonConvert.DeserializeObject<Greeter>(data);

  try
  {
    var response = await client.GreetAsync(new GreetingRequest { Greeting = new Greeting { FirstName = greeter.first_name, LastName = greeter.last_name } });
    return await Task.FromResult(response.Result);
  }
  catch (Exception e)
  {
    return e.Message;
  }
});

greetApi.MapGet("/", () =>
{
  Console.WriteLine("MapGet");

  return "Client got greet.";
});

app.Run();