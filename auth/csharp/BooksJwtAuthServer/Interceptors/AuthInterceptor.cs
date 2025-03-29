using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Interceptors;

public class AuthInterceptor : Interceptor
{
    private readonly IDictionary<string, string[]> _accessibleRoles;

    public AuthInterceptor()
    {
        _accessibleRoles = Config.Configs.AccessibleRoles;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        CheckAuthorization(context);

        return await continuation(request, context);
    }

    private void CheckAuthorization(ServerCallContext context)
    {
        var methodName = context.Method;
        if (!_accessibleRoles.ContainsKey(methodName))
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, $"No permissions defined for {methodName}"));
        }

        var httpContext = context.GetHttpContext();

        bool hasRole = IsInRole(httpContext.User, _accessibleRoles[methodName]);      
        if (!hasRole)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, $"User {httpContext.User.Identity.Name} has no permission for {context.Method}"));
        }
    }

    private static bool IsInRole(ClaimsPrincipal user, string[] roles)
    {
        foreach (var role in roles)
        {
            if (user.IsInRole(role))
            {
                return true;
            }
        }
        return false;
    }
}
