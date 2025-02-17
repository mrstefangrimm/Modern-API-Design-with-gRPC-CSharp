using Grpc.Core;
using Model;
using Prot;
using Repo;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcBooksServer;

public class GrpcBooksService : BookService.BookServiceBase
{
    private readonly BookRepository _bookRepo;

    public GrpcBooksService(BookRepository bookRepo)
    {
        _bookRepo = bookRepo;
    }

    public override Task<AddBookResponse> AddBook(Book request, ServerCallContext context)
    {

        // Not in the book. Error handling
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required."));
        }

        // Implementing recovery interceptor, not implemented
        if (request.Isbn == 0)
        {
            Environment.Exit(-1);
        }

        var book = new DBBook
        {
            Isbn = request.Isbn,
            Name = request.Name,
            Publisher = request.Publisher
        };

        _bookRepo.AddBook(book);

        return Task.FromResult(new AddBookResponse
        {
            Status = $"book with isbn({book.Isbn}), name({book.Name}), publisher({book.Publisher}) added successfully"
        });
    }

    public override Task<UpdateBookResponse> UpdateBook(Book request, ServerCallContext context)
    {
        var book = new DBBook
        {
            Isbn = request.Isbn,
            Name = request.Name,
            Publisher = request.Publisher
        };
        _bookRepo.UpdateBook(book);

        return Task.FromResult(new UpdateBookResponse
        {
            Status = $"book with isbn({book.Isbn}), name({book.Name}), publisher({book.Publisher}) updated successfully"
        });
    }

    public override Task<ListBooksRespose> ListBooks(Empty request, ServerCallContext context)
    {
        var books = _bookRepo.GetAllBooks().Select(dbBook => new Book
        {
            Isbn = dbBook.Isbn,
            Name = dbBook.Name,
            Publisher = dbBook.Publisher
        });

        var resp = new ListBooksRespose();
        resp.Books.AddRange(books);

        return Task.FromResult(resp);
    }

    public override Task<Book> GetBook(GetBookRequest request, ServerCallContext context)
    {

        // Resilience, mitigating with timeout
        if (request.Isbn == 12347 && DateTime.Now.Second % 4 == 0)
        {
            Thread.Sleep(3000);
        }

        var book = _bookRepo.GetBook(request.Isbn);

        return Task.FromResult(new Book
        {
            Isbn = book.Isbn,
            Name = book.Name,
            Publisher = book.Publisher
        });
    }

    public override Task<RemoveBookResponse> RemoveBook(RemoveBookRequest request, ServerCallContext context)
    {
        _bookRepo.RemoveBook(request.Isbn);

        return Task.FromResult(new RemoveBookResponse
        {
            Status = "book with isbn(%d) removed successfully"
        });
    }
}
