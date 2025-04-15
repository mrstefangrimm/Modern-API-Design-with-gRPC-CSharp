using GrpcBooksServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.ConfigureKestrel((context, serverOptions) =>
//{
//    serverOptions.Listen(IPAddress.Parse("127.0.0.1"), 5001, listenOptions =>
//    {
//        listenOptions.UseHttps("server.pfx", "grpc");
//    });
//});

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<GrpcBooksService>();
app.MapGrpcReflectionService();

app.Run();
