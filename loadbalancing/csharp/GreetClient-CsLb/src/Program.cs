using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
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
Console.WriteLine($"Expecting static load balanced grpc greet server address: {grpcAddress}");

var grpcHost_gs1 = builder.Configuration["GprcGreetServerHost1"]!;
var grpcPort_gs1 = int.Parse(builder.Configuration["GprcGreetServerPort1"]!);
Console.WriteLine($"Expecting grpc greet server 1 address: {grpcHost_gs1}:{grpcPort_gs1}");

var grpcHost_gs2 = builder.Configuration["GprcGreetServerHost2"]!;
var grpcPort_gs2 = int.Parse(builder.Configuration["GprcGreetServerPort2"]!);
Console.WriteLine($"Expecting grpc greet server 2 address: {grpcHost_gs2}:{grpcPort_gs2}");

var factory = new StaticResolverFactory(addr => new[]
{
    new BalancerAddress(grpcHost_gs1, grpcPort_gs1),
    new BalancerAddress(grpcHost_gs2, grpcPort_gs2),
});
builder.Services.AddSingleton<ResolverFactory>(factory);

using var channel = GrpcChannel.ForAddress(
  grpcAddress,
  new GrpcChannelOptions
  {
    Credentials = Grpc.Core.ChannelCredentials.Insecure,
    ServiceProvider = builder.Services.BuildServiceProvider(),
    ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } }
  });
var client = new GreetService.GreetServiceClient(channel);

var app = builder.Build();

var greetApi = app.MapGroup("/");

greetApi.MapPost("/greet", async Task<string> (HttpRequest request) =>
{
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