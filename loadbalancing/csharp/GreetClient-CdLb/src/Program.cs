using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Proto;
using System;
using System.IO;
using System.Threading.Tasks;

var builder = WebApplication.CreateSlimBuilder(args);

Console.WriteLine("Hello.");

var grpcAddress = builder.Configuration["GrpcGreetServerUrl"]!;
Console.WriteLine($"Expecting grpc greet server address: {grpcAddress}");
Console.WriteLine($"LoadBalancingConfig: RoundRobinConfig");

var channel = GrpcChannel.ForAddress(
    grpcAddress,
    new GrpcChannelOptions
    {
      Credentials = ChannelCredentials.Insecure,
      ServiceProvider = builder.Services.BuildServiceProvider(),
      ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } }
    });
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

  return "+++++";
});

app.Run();