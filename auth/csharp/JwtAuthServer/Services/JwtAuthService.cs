using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using Prot;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrpcJwtAuthServer;

public class JwtAuthService : AuthService.AuthServiceBase
{
    private static readonly JwtSecurityTokenHandler JwtTokenHandler = new JwtSecurityTokenHandler();
    private static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("long-secret-is-required-256-minimum"));

    public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        return Task.FromResult(new LoginResponse
        {
            AccessToken = GenerateJwtToken(request.Username)
        });
    }

    private static string GenerateJwtToken(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidOperationException("Name is not specified.");
        }

        var claims = new[] { new Claim(ClaimTypes.Name, name) };
        var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);
        return JwtTokenHandler.WriteToken(token);
    }
}
