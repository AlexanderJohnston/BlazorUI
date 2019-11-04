using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MLSection;
//GIFLENS-https://i.pinimg.com/originals/1c/99/44/1c994411efbfb66d96caebda7e4dec70.gif
namespace BlazorUI.Server
{
  public class EngineControlUnitHostedService : IHostedService
  {
  private Reader _reader;    
    public Task StartAsync(CancellationToken cancellationToken)
    {
      return new Reader().ReadDiskImages();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      throw new System.NotImplementedException();
    }
  }
}