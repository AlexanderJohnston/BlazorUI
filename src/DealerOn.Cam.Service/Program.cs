using System.Threading.Tasks;
using Totem.App.Service;

namespace DealerOn.Cam.Service
{
  /// <summary>
  /// The entry point of the CAM timeline service
  /// </summary>
  class Program
  {
    public static Task Main() =>
      ServiceApp.Run<CamArea>(services => services.AddCam());
  }
}