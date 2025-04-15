using Grpc.Core;
using Prot;
using System.Threading.Tasks;

namespace GrpcBooksServer;

public class GrpcBooksService : BookService.BookServiceBase
{
    public override Task<Book> GetBook(GetBookRequest request, ServerCallContext context)
    {
        var user = context.GetHttpContext().User;

        return Task.FromResult(new Book
        {
            Isbn = 123456,
            Name = "Secured gRPC",
            Publisher = "OAVA"
        });
    }
}
