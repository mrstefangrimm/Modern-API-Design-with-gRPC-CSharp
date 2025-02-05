using Grpc.Core;
using Model;
using Prot;
using Repo;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcReviewServer;

public class GrpcReviewService : ReviewService.ReviewServiceBase
{
  private readonly BookRepository _bookRepo;

  public GrpcReviewService(BookRepository bookRepo)
  {
    _bookRepo = bookRepo;
  }

  public override Task<GetBookReviewsResponse> GetBookReviews(GetBookReviewsRequest request, ServerCallContext context)
  {
    var reviews = _bookRepo.GetAllReviews(request.Isbn).Select(dbReview => new Review {
      Comment = dbReview.Comment,
      Reviewer = dbReview.Reviewer,
      Rating = dbReview.Rating
    });

    var resp = new GetBookReviewsResponse();
    resp.Reviews.AddRange(reviews);

    return Task.FromResult(resp);
  }

  public override Task<SubmitReviewResponse> SubmitReviews(SubmitReviewRequest request, ServerCallContext context)
  {
    var review = new DBReview {
      Isbn = request.Isbn,
      Comment = request.Comment,
      Reviewer = request.Reviewer,
      Rating = request.Rating
    };

    _bookRepo.AddReview(review);

    return Task.FromResult(new SubmitReviewResponse {
      Status = $"review for book({request.Isbn}) submitted successfully"
    });
  }

  public Task<AddBookResponse> AddBook(Book request, ServerCallContext context)
  {
    var book = new DBBook {
      Isbn = request.Isbn,
      Name = request.Name,
      Publisher = request.Publisher
    };
    _bookRepo.AddBook(book);

    return Task.FromResult(new AddBookResponse {
      Status = $"book with isbn({book.Isbn}), name({book.Name}), publisher({book.Publisher}) added successfully"
    });
  }

  public Task<UpdateBookResponse> UpdateBook(Book request, ServerCallContext context)
  {
    var book = new DBBook {
      Isbn = request.Isbn,
      Name = request.Name,
      Publisher = request.Publisher
    };
    _bookRepo.UpdateBook(book);

    return Task.FromResult(new UpdateBookResponse {
      Status = $"book with isbn({book.Isbn}), name({book.Name}), publisher({book.Publisher}) updated successfully"
    });
  }

  public Task<ListBooksRespose> ListBooks(Empty request, ServerCallContext context)
  {
    var books = _bookRepo.GetAllBooks().Select(dbBook => new Book {
      Isbn = dbBook.Isbn,
      Name = dbBook.Name,
      Publisher = dbBook.Publisher
    });

    var resp = new ListBooksRespose();
    resp.Books.AddRange(books);

    return Task.FromResult(resp);
  }

  public Task<Book> GetBook(GetBookRequest request, ServerCallContext context)
  {
    var book = _bookRepo.GetBook(request.Isbn);

    return Task.FromResult(new Book {
      Isbn = book.Isbn,
      Name = book.Name,
      Publisher = book.Publisher
    });
  }

  public Task<RemoveBookResponse> RemoveBook(RemoveBookRequest request, ServerCallContext context)
  {
    _bookRepo.RemoveBook(request.Isbn);

    return Task.FromResult(new RemoveBookResponse {
      Status = "book with isbn(%d) removed successfully"
    });
  }
}
