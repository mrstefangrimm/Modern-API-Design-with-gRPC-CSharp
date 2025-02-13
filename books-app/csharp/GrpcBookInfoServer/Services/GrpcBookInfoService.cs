using Grpc.Core;
using Model;
using Prot;
using System.Threading.Tasks;

namespace GrpcBookInfoServer;

public class GrpcBookInfoService : BookInfoService.BookInfoServiceBase
{
  private readonly BookService.BookServiceClient _bookServerClient;
  private readonly ReviewService.ReviewServiceClient _reviewServerClient;


  public GrpcBookInfoService(BookService.BookServiceClient bookServerClient, ReviewService.ReviewServiceClient reviewServerClient)
  {
    _bookServerClient = bookServerClient;
    _reviewServerClient = reviewServerClient;
  }

  public override Task<GetBookInfoResponse> GetBookInfoWithReviews(GetBookInfoRequest request, ServerCallContext context)
  {
    var book = _bookServerClient.GetBook(new GetBookRequest { Isbn = request.Isbn });
    var reviewResp = _reviewServerClient.GetBookReviews(new GetBookReviewsRequest { Isbn = request.Isbn });

    var resp = new GetBookInfoResponse {
      Isbn = request.Isbn,
      Name = book.Name,
      Publisher = book.Publisher
    };
    resp.Reviews.AddRange(reviewResp.Reviews);

    return Task.FromResult(resp);
  }
}
