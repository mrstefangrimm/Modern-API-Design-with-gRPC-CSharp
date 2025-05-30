using Grpc.Core;
using Grpc.Net.Client;
using Prot;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

var handler = new HttpClientHandler();
var clientCertificate = X509CertificateLoader.LoadPkcs12FromFile(@"Certs\client.pfx", "grpc");
handler.ClientCertificates.Add(clientCertificate);

var channelOptions = new GrpcChannelOptions
{
    HttpHandler = handler
};

using var channel = GrpcChannel.ForAddress("https://localhost:5001", channelOptions);
var client = new BookService.BookServiceClient(channel);

var book = await client.GetBookAsync(new GetBookRequest { Isbn = 123456 });
Console.WriteLine($"Isbn:{book.Isbn}, Name:{book.Name}, Publisher:{book.Publisher}");

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
