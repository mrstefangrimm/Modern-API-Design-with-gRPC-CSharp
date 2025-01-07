using Grpc.Core;
using Proto;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
  public class GrpcMaxService : MaxService.MaxServiceBase
  {
    public override Task FindMax(IAsyncStreamReader<MaxRequest> requestStream, IServerStreamWriter<MaxResponse> responseStream, ServerCallContext context)
    {
      int max = 0;
      bool isResponding = true;

      var respondTask = Task.Factory.StartNew(() =>
      {
        while (isResponding)
        {
          Thread.Sleep(2000);

          if (context.CancellationToken.IsCancellationRequested)
          {
            throw new TaskCanceledException();
          }

          var resp = new MaxResponse { Max = max };
          responseStream.WriteAsync(resp);
        }
      });

      while (requestStream.MoveNext(context.CancellationToken).Result)
      {
        int num = requestStream.Current.Number;
        if (max < num)
        {
          max = num;
        }
      }

      isResponding = false;
      respondTask.Wait();

      return Task.CompletedTask;
    }
  }
}
