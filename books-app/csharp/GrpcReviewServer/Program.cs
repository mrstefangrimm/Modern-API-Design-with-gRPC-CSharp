using Db;
using GrpcReviewServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repo;
using System;

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
app.MapGrpcService<GrpcReviewService>();

app.Run();

