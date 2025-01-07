using Grpc.Core;
using Proto;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
  public class GrpcAverageService : AverageService.AverageServiceBase
  {
    public override Task<AverageResponse> FindAverage(IAsyncStreamReader<AverageRequest> requestStream, ServerCallContext context)
    {
      int sum = 0;
      int count = 0;

      while (requestStream.MoveNext(context.CancellationToken).Result)
      {
        count += 1;
        sum += requestStream.Current.Number;
      }

      var resp = new AverageResponse { Average = sum / count };
      return Task.FromResult(resp);
    }
  }
}
