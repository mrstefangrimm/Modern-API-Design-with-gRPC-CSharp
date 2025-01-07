using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<GrpcAverageService>();

app.Run();
