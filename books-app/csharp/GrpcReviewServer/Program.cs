using Db;
using GrpcReviewServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<BookDbContext>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<GrpcReviewService>();
app.MapGrpcReflectionService();

app.Run();

