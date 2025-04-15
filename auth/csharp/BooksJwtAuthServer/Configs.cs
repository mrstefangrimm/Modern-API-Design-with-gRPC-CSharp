using System.Collections.Generic;

namespace Config;

public static class Configs
{
    private static string _bookServicePath = "/prot.BookService/";
    private static string _bookReviewServicePath = "/prot.ReviewService/";
    private static string _bookInfoServicePath = "/prot.BookInfoService/";

    public static IDictionary<string, string[]> AccessibleRoles { get; } = new Dictionary<string, string[]>
    {
        { _bookServicePath + "AddBook", new [] { "admin" } },
        { _bookServicePath + "UpdateBook", new [] { "admin" } },
        { _bookServicePath + "RemoveBook", new [] { "admin" } },

        { _bookServicePath + "GetBook", new [] { "admin", "user" } },
        { _bookReviewServicePath + "GetBookReviews", new [] { "admin", "user" } },
        { _bookReviewServicePath + "SubmitReviews", new [] { "admin", "user" } },
        { _bookInfoServicePath + "GetBookInfoWithReviews", new [] { "admin", "user" } },
    };
}
