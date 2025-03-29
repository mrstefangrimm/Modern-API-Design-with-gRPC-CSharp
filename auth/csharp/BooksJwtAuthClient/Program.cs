using Grpc.Core;
using Grpc.Net.Client;
using Prot;
using System;
using System.Net.Http;

using var authenticationChannel = GrpcChannel.ForAddress("https://localhost:5101");
var authenticationClient = new AuthService.AuthServiceClient(authenticationChannel);

var loginResponse = await authenticationClient.LoginAsync(new LoginRequest { Username = "admin1", Password = "12345" });

var channelOptions = new GrpcChannelOptions
{
    Credentials = ChannelCredentials.SecureSsl,
    HttpClient = new HttpClient
    {
        DefaultRequestHeaders =
        {
            { "authorization", $"Bearer {loginResponse.AccessToken}" }
        }
    }
};

using var channel = GrpcChannel.ForAddress("https://localhost:5001", channelOptions);
var client = new BookService.BookServiceClient(channel);

var book = await client.GetBookAsync(new GetBookRequest { Isbn = 123456 });
Console.WriteLine($"Isbn:{book.Isbn}, Name:{book.Name}, Publisher:{book.Publisher}");

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
