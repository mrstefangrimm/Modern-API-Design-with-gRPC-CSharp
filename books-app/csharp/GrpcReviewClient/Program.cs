// Not in the book

using Grpc.Net.Client;
using Prot;
using System;

var reviewClient = new ReviewService.ReviewServiceClient(GrpcChannel.ForAddress("https://localhost:5101"));

await reviewClient.SubmitReviewsAsync(new SubmitReviewRequest {
  Isbn = 12348,
  Reviewer = "brother",
  Comment = "Not too bad",
  Rating = 7
});

await reviewClient.SubmitReviewsAsync(new SubmitReviewRequest {
  Isbn = 12348,
  Reviewer = "other",
  Comment = "Yep",
  Rating = 8
});

var reviewsResp = await reviewClient.GetBookReviewsAsync(new GetBookReviewsRequest { Isbn = 12348 });
foreach (var r in reviewsResp.Reviews)
{
  Console.WriteLine($"Reviewer:{r.Reviewer}, Comment:{r.Comment}, Rating:{r.Rating}");
}

Console.WriteLine("Shutting down");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
