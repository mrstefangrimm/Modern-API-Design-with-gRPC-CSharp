﻿using Grpc.Core.Interceptors;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace GrpcBooksClient.Interceptors;

public class ErrorHandlerInterceptor : Interceptor {
  public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
      TRequest request,
      ClientInterceptorContext<TRequest, TResponse> context,
      AsyncUnaryCallContinuation<TRequest, TResponse> continuation) {
    var call = continuation(request, context);

    return new AsyncUnaryCall<TResponse>(
        HandleResponse(call.ResponseAsync),
        call.ResponseHeadersAsync,
        call.GetStatus,
        call.GetTrailers,
        call.Dispose);
  }

  public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation) {
    try {
      return await continuation(request, context);
    }
    catch (Exception ex) {
      throw new InvalidOperationException("Custom error", ex);
    }

  }

  private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner) {
    try {
      return await inner;
    }
    catch (Exception ex) {
      throw new InvalidOperationException("Custom error", ex);
    }
  }
}
