using Grpc.Core;
using Proto;
using System;
using System.Threading.Tasks;

namespace GreetServer.Services
{
  class GrpcGreetService : GreetService.GreetServiceBase
  {
    public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
    {
      Console.WriteLine("Service got greet.");

      string podIp = Environment.GetEnvironmentVariable("POD_IP") ?? string.Empty;
      string podName = Environment.GetEnvironmentVariable("POD_NAME") ?? string.Empty;

      return Task.FromResult(new GreetingResponse { Result = $"Hello, {request.Greeting.FirstName} {request.Greeting.LastName} from pod: name({podName}), ip({podIp})." });
    }
  }
}
