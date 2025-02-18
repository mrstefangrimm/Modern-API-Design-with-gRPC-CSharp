using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;

namespace GrpcBooksServer.Interceptors;

public class BasicAuthInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var credentials = ExtractAuth(context);
        if (!credentials.Ok || !IsValidUser(credentials.Username, credentials.Password))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid credentials"));
        }

        return await continuation(request, context);
    }

    private static (string Username, string Password, bool Ok) ExtractAuth(ServerCallContext context)
    {
        var httpHeaders = context.GetHttpContext().Request.Headers;
        var authHeaders = httpHeaders["authorization"];
        if (authHeaders.Count == 0)
        {
            return ("", "", false);
        }

        var authParts = authHeaders.ToString().Split(" ");
        if (authParts.Length != 2 && authParts[0].ToLower() != "basic")
        {
            return ("", "", false);
        }

        byte[] data = Convert.FromBase64String(authParts[1]);
        string decoded = System.Text.Encoding.UTF8.GetString(data);

        var creds = decoded.Split(":");
        if (creds.Length != 2)
        {
            return ("", "", false);
        }

        return (creds[0], creds[1], true);
    }

    private static bool IsValidUser(string username, string password)
    {
        // this logic to validate needs to be replaced.
        // For simplicity, we have a hard-coded here.
        return username == "uname" && password == "safePass";
    }
}
