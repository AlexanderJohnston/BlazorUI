using System.Threading.Tasks;

namespace DealerOn.Cam.Data
{
  /// <summary>
  /// Describes a Toyota-hosted CAM manifest declaring a dealer's campaigns
  /// </summary>
  public interface IDealerManifestFile
  {
    Task<DealerManifest> Download(Manifest.Dealer dealer, Pages pages);
  }
}