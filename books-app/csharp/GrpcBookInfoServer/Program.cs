using Grpc.Net.Client;
using GrpcBookInfoServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prot;

var bookClient = new BookService.BookServiceClient(GrpcChannel.ForAddress("https://localhost:5001"));
var reviewClient = new ReviewService.ReviewServiceClient(GrpcChannel.ForAddress("https://localhost:5101"));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton(sp => bookClient);
builder.Services.AddSingleton(sp => reviewClient);
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<GrpcBookInfoService>();
app.MapGrpcReflectionService();

app.Run();
