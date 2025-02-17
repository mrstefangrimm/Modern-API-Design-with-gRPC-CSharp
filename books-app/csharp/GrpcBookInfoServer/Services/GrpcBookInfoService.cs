using Grpc.Core;
using GrpcBookInfoServer.Resilience;
using Prot;
using System;
using System.Threading.Tasks;

namespace GrpcBookInfoServer;

public class GrpcBookInfoService : BookInfoService.BookInfoServiceBase
{
    private readonly BookService.BookServiceClient _bookServerClient;
    private readonly ReviewService.ReviewServiceClient _reviewServerClient;
    private readonly CircuitBreaker _circuitBreaker;

    public GrpcBookInfoService(BookService.BookServiceClient bookServerClient, ReviewService.ReviewServiceClient reviewServerClient, CircuitBreaker circuitBreaker)
    {
        _bookServerClient = bookServerClient;
        _reviewServerClient = reviewServerClient;
        _circuitBreaker = circuitBreaker;
    }

    public override Task<GetBookInfoResponse> GetBookInfoWithReviews(GetBookInfoRequest request, ServerCallContext context)
    {
        DateTime deadline = DateTime.MaxValue;
        if (request.Isbn == 12347)
        {
            deadline = DateTime.UtcNow.AddSeconds(1);
        }

        Func<(Book, GetBookReviewsResponse)> retry = () =>
        {
            var book = _bookServerClient.GetBook(new GetBookRequest { Isbn = request.Isbn }, deadline: deadline);
            var reviewResp = _reviewServerClient.GetBookReviews(new GetBookReviewsRequest { Isbn = request.Isbn });

            return (book, reviewResp);
        };

        var retryResp = _circuitBreaker.Execute<(Book, GetBookReviewsResponse)>(() =>
        {
            return retry.WithRetry(3, TimeSpan.FromSeconds(2));
        });

        var resp = new GetBookInfoResponse
        {
            Isbn = request.Isbn,
            Name = retryResp.Item1.Name,
            Publisher = retryResp.Item1.Publisher
        };
        resp.Reviews.AddRange(retryResp.Item2.Reviews);

        return Task.FromResult(resp);
    }
}
