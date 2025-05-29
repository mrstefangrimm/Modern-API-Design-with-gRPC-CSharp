using Config;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using Prot;
using Repo;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GrpcJwtAuthServer;

public class JwtAuthService : AuthService.AuthServiceBase
{
    private static readonly JwtSecurityTokenHandler JwtTokenHandler = new JwtSecurityTokenHandler();
    private readonly IUserStore _userStore;

    public JwtAuthService(IUserStore userStore)
    {
        _userStore = userStore;
    }

    public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        try
        {
            var user = _userStore.Find(request.Username);
            if (user == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, $"Cannot find user {request.Username}."));
            }

            if (!user.IsCorrectPassword(request.Password))
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password."));
            }

            return Task.FromResult(new LoginResponse
            {
                AccessToken = GenerateJwtToken(user.Username, user.Role)
            });
        }
        catch (Exception)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Login failed."));
        }
    }

    private static string GenerateJwtToken(string name, string role)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        var claims = new[] { new Claim(ClaimTypes.Name, name), new Claim(ClaimTypes.Role, role) };
        var credentials = new SigningCredentials(Configs.SecurityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims,
            expires: DateTime.Now.Add(Configs.TokenDuration),
            signingCredentials: credentials);
        return JwtTokenHandler.WriteToken(token);
    }
}
