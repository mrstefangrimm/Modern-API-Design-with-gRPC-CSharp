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
  Console.WriteLine($"MapPost /greet, grpc address:{grpcAddress}.");

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
  Console.WriteLine($"MapGet /.");

  string podIp = Environment.GetEnvironmentVariable("POD_IP") ?? string.Empty;
  string podName = Environment.GetEnvironmentVariable("POD_NAME") ?? string.Empty;

  return $"Client got greet with pod: name({podName}), ip({podIp}).";
});

greetApi.MapGet("/health-readiness", () =>
{
  Console.WriteLine($"MapGet /health-readiness.");

  return "health-ready";
});

greetApi.MapGet("/health-liveness", () =>
{
  Console.WriteLine($"MapGet /health-liveness.");

  return "health-alive";
});

greetApi.MapGet("/health-startup", () =>
{
  Console.WriteLine($"MapGet /health-startup.");

  return "health-started";
});


app.Run();