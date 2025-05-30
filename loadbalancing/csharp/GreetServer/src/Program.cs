using GreetServer.Services;
using Grpc.Core;
using Proto;
using System;

var portString = Environment.GetEnvironmentVariable("GRPC_PORT");
int port = portString != null ? int.Parse(portString) : 50051;
var host = Environment.GetEnvironmentVariable("GRPC_HOST") ?? "0.0.0.0";

var serverCredentials = ServerCredentials.Insecure;

var server = new Server()
{
  Services = { GreetService.BindService(new GrpcGreetService()) },
  Ports = { new ServerPort(host, port, serverCredentials) },
};

server.Start();
Console.WriteLine($"Started grpc server {host}:{port}");

server.ShutdownTask.Wait();
