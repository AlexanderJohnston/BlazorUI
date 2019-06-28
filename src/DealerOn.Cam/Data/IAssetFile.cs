using System.Threading.Tasks;
using Totem.IO;

namespace DealerOn.Cam.Data
{
  /// <summary>
  /// Describes a Toyota-hosted asset file for a campaign
  /// </summary>
  public interface IAssetFile
  {
    Task Download(HttpLink link);
  }
}