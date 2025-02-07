using Db;
using GrpcBooksServer;
using GrpcBooksServer.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repo;

var builder = WebApplication.CreateBuilder(args);

//https://learn.microsoft.com/en-us/aspnet/core/grpc/aspnetcore?view=aspnetcore-9.0&tabs=visual-studio
//builder.WebHost.ConfigureKestrel(options => {
//  options.Listen(IPAddress.Any, 5050, listenOptions => {
//    listenOptions.Protocols = HttpProtocols.Http2;
//  });
//});

builder.Services.AddGrpc(options => {
  // book, section: Implementing a logging interceptor
  options.Interceptors.Add<LoggingInterceptor>();
});


builder.Services.AddDbContext<BookDbContext>();

builder.Services.AddScoped<BookRepository>();

var app = builder.Build();
app.MapGrpcService<GrpcBooksService>();

app.Run();

