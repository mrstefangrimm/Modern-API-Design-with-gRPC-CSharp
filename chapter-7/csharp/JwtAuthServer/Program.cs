using GrpcJwtAuthServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var userStore = new UserStore();
Config.Configs.SeedUsers(userStore);

builder.Services.AddSingleton<IUserStore>(userStore);

var app = builder.Build();

app.MapGrpcService<JwtAuthService>();
app.MapGrpcReflectionService();

app.Run();
