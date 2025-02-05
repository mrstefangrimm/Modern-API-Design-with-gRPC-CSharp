// Not in the book

using Grpc.Net.Client;
using Prot;
using System;

var client = new BookInfoService.BookInfoServiceClient(GrpcChannel.ForAddress("https://localhost:5201"));

var resp = await client.GetBookInfoWithReviewsAsync(new GetBookInfoRequest { Isbn = 12348 });

Console.WriteLine($"Isbn:{resp.Isbn}, Name:{resp.Name}, Publisher:{resp.Publisher}");

foreach (var r in resp.Reviews)
{
  Console.WriteLine($"  Reviewer:{r.Reviewer}, Comment:{r.Comment}, Rating:{r.Rating}");
}

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
