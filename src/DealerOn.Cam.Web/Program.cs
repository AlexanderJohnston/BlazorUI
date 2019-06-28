using System.Threading.Tasks;
using Totem.App.Web;

namespace DealerOn.Cam.Web
{
  /// <summary>
  /// The entry point of the CAM web server
  /// </summary>
  public class Program
  {
    public static Task Main() => WebApp.Run<CamArea>();
  }
}