using System.IO;
using System.Threading.Tasks;
using Totem.IO;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// Describes the DealerOn folder containing downloaded assets
  /// </summary>
  public interface IAssetFolder
  {
    Task WriteFile(HttpLink link, Stream data);
  }
}