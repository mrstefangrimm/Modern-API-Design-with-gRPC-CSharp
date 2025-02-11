using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;

namespace GrpcBooksServer.Interceptors;

public class UniqueExceptionInterceptor : Interceptor
{
  public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
  {
    try
    {
      return await continuation(request, context);
    }
    catch (Exception ex)
    {
      throw new RpcException(new Status(StatusCode.Aborted, ex.Message, ex));
    }
  }
}
