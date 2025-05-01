using GreetServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace GreetServer
{
  class Program
  {
    public static void Main(string[] args)
    {
      var portString = Environment.GetEnvironmentVariable("GRPC_PORT");
      int port = portString != null ? int.Parse(portString) : 50051;
      var host = Environment.GetEnvironmentVariable("GRPC_HOST") ?? "0.0.0.0";

      //var serverCredentials = ServerCredentials.Insecure;

      //var server = new Server()
      //{
      //  Services = { GreetService.BindService(new GrpcGreetService()) },
      //  Ports = { new ServerPort(host, port, serverCredentials) },
      //};

      //server.Start();
      //Console.WriteLine($"Started grpc server {host}:{port}");

      //server.ShutdownTask.Wait();

      var builder = WebApplication.CreateBuilder(args);

      builder.WebHost.ConfigureKestrel(options =>
      {
        options.Listen(IPAddress.Parse(host), port, listenOptions =>
        {
          listenOptions.Protocols = HttpProtocols.Http2;
        });
      });

      builder.Services.AddGrpc();
      var app = builder.Build();

      app.MapGrpcService<GrpcGreetService>();
      //app.MapGrpcReflectionService();

      app.Run();
    }
  }
}
