using GrpcBooksServer;
using GrpcBooksServer.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options => {
  options.Interceptors.Add<BasicAuthInterceptor>();
});
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<GrpcBooksService>();
app.MapGrpcReflectionService();

app.Run();
