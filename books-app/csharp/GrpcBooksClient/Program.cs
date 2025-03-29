using Grpc.Net.Client;
using Prot;
using System;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new BookService.BookServiceClient(channel);

// Not in the book. Error handling
var invalidArgumentBook = new Book {
  Isbn = 12345,
  Name = "",
  Publisher = "random house business books"
};
var res = await ExceptionExtension.CallWithTryCatchAsync<Book, AddBookResponse>(arg => {
  return client.AddBookAsync(arg);
}, invalidArgumentBook);

Console.WriteLine($"Invalid argument book returned '{res}'");

// Not in the book. recovery implementation when the server crashes, not implemented.
//var serverCrashingBook = new Book {
//  Isbn = 0,
//  Name = "atomic habits",
//  Publisher = "random house business books"
//};
//_ = await ExceptionExtension.CallWithTryCatchAsync<Book, AddBookResponse>(arg => {
//  return client.AddBookAsync(arg);
//}, serverCrashingBook);

var book = new Book {
  Isbn = 12348,
  Name = "atomic habits",
  Publisher = "random house business books"
};

await client.AddBookAsync(book);

var listResp = await client.ListBooksAsync(new Empty());

bool found = false;
foreach (var b in listResp.Books)
{
  Console.WriteLine($"Isbn:{b.Isbn}, Name:{b.Name}, Publisher:{b.Publisher}");

  if (b.Isbn == book.Isbn && b.Name == book.Name && b.Publisher == book.Publisher)
  {
    found = true;
  }
}

if (found)
{
  Console.WriteLine("Book sent through 'AddBook' request was verified to have been added while listing books.");
}

book.Name = "atomic habits vol-2";
await client.UpdateBookAsync(book);

var updatedBook = await client.GetBookAsync(new GetBookRequest { Isbn = 12348 });
Console.WriteLine($"Isbn:{updatedBook.Isbn}, Name:{updatedBook.Name}, Publisher:{updatedBook.Publisher}");

await client.RemoveBookAsync(new RemoveBookRequest { Isbn = 12348 });

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
