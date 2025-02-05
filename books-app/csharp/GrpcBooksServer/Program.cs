using Db;
using GrpcBooksServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

var app = builder.Build();
app.MapGrpcService<GrpcBooksService>();

app.Run();

