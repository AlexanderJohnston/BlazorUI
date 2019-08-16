using System.Threading.Tasks;

namespace BlazorUI.Client.Campaign.Data
{
  /// <summary>
  /// Describes a Toyota-hosted CAM manifest declaring a dealer's campaigns
  /// </summary>
  public interface IDealerManifestFile
  {
    Task<DealerManifest> Download(Manifest.Dealer dealer, Pages pages);
  }
}