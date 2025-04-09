using GrpcBooksServer;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(kestrelOptions =>
{
    kestrelOptions.ConfigureHttpsDefaults(options =>
    {
        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
        options.ClientCertificateValidation = (cert, chain, policyErrors) =>
        {
            // Certificate validation logic here
            // Return true if the certificate is valid or false if it is invalid

            var localClientCert = X509CertificateLoader.LoadCertificateFromFile(@"Cert\client.pfx");

            return policyErrors == System.Net.Security.SslPolicyErrors.None && cert.Thumbprint == localClientCert.Thumbprint;
        };
    });
});

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        // Not recommended in production environments. The example is using a self-signed test certificate.
        options.RevocationMode = X509RevocationMode.NoCheck;
        options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
        options.AllowedCertificateTypes = CertificateTypes.All;
    });

var app = builder.Build();

app.UseAuthentication();

app.MapGrpcService<GrpcBooksService>();
app.MapGrpcReflectionService();

app.Run();
