// book, section: Implementing a logging interceptor

using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GrpcBooksServer.Interceptors;

public class LoggingInterceptor : Interceptor
{
  private readonly ILogger<LoggingInterceptor> _logger;

  public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
  {
    _logger = logger;
  }
  
  public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
  {
    try
    {
      var stopWatch = new Stopwatch();
      stopWatch.Start();
      _logger.Log(LogLevel.Information, $"BEGIN {context.Method}");
      var resp = await continuation(request, context);
      stopWatch.Stop();
      _logger.Log(LogLevel.Information, $"End {context.Method}, work: {stopWatch.Elapsed.TotalMilliseconds} ms.");

      return resp;
    }
    catch (Exception ex)
    {
      // Note: The gRPC framework also logs exceptions thrown by handlers to .NET logging.
      _logger.LogCritical(ex, $"Error thrown by {context.Method}.");

      throw;
    }
  }

}
