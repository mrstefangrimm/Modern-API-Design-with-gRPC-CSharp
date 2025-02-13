using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace GrpcBooksClient.Interceptors;

public static class ExceptionExtension
{
  public static TResponse CallWithTryCatch<TRequest, TResponse>(Func<TRequest, TResponse> rpcCall, TRequest arg)
  {
    try
    {
      return rpcCall(arg);
    }
    catch (RpcException rpcEx)
    {
      return default(TResponse);
    }
  }

  public static async Task<TResponse> CallWithTryCatchAsync<TRequest, TResponse>(Func<TRequest, AsyncUnaryCall<TResponse>> rpcCallAsync, TRequest arg)
  {
    try
    {
      var result = await rpcCallAsync(arg);
      return result;
    }
    catch (RpcException rpcEx)
    {
      Console.WriteLine(rpcEx.ToString());
      return default(TResponse);
    }
  }
}
