using Grpc.Core;
using Grpc.Net.Client;
using Prot;
using System;
using System.Net.Http;

var channelOptions = new GrpcChannelOptions
{
    Credentials = ChannelCredentials.SecureSsl,
    HttpClient = new HttpClient
    {
        DefaultRequestHeaders =
        {
            { "authorization", "Basic dW5hbWU6c2FmZVBhc3M=" } // "uname:safePass" encoded with https://www.base64encode.org/
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
