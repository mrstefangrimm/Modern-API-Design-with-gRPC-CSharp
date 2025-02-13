using Db;
using GrpcBooksServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repo;
using System;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//https://learn.microsoft.com/en-us/aspnet/core/grpc/aspnetcore?view=aspnetcore-9.0&tabs=visual-studio
//builder.WebHost.ConfigureKestrel(options => {
//  options.Listen(IPAddress.Any, 5050, listenOptions => {
//    listenOptions.Protocols = HttpProtocols.Http2;
//  });
//});

builder.Services.AddGrpc();


builder.Services.AddDbContext<BookDbContext>();

builder.Services.AddScoped<BookRepository>();

builder.Services.AddGrpcReflection(); // Used by grpcurl

var app = builder.Build();

app.MapGrpcService<GrpcBooksService>();

var env = app.Environment;
//if (env.IsDevelopment())
{
  app.MapGrpcReflectionService();
  //app.MapGrpcReflectionService().AllowAnonymous();
}

app.Run();

