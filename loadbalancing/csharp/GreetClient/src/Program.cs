using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using Proto;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

var portString = Environment.GetEnvironmentVariable("PORT");
int port = portString != null ? int.Parse(portString) : 8080;
var host = Environment.GetEnvironmentVariable("HOST") ?? "0.0.0.0";

var grpcPortString = Environment.GetEnvironmentVariable("GATEWAY_PORT");
int grpcPort = grpcPortString != null ? int.Parse(grpcPortString) : 50051;
var grpcHost = Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "greet-server"; //minikube getting started:greet-server, kube: greetserver, kubectl get services; // when compose is used: "host.docker.internal";

Console.WriteLine("Hello");

//var channelCredentials = ChannelCredentials.Insecure;
//var channel = new Channel($"{host}:{port}", channelCredentials);

string address = $"http://{grpcHost}:{grpcPort}";
Console.WriteLine(address);
using var channel = GrpcChannel.ForAddress(address);
var client = new GreetService.GreetServiceClient(channel);

var builder = WebApplication.CreateSlimBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
  options.Listen(IPAddress.Parse(host), port, listenOptions =>
  {
    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
  });
});
var app = builder.Build();

var greetApi = app.MapGroup("/");

greetApi.MapPost("/greet", async Task<string> (HttpRequest request) =>
{
  Console.WriteLine($"MapPost, grpc address:{address}");

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