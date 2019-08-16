using System.Threading.Tasks;

namespace BlazorUI.Client.Campaign.Data
{
  /// <summary>
  /// Describes the Toyota-hosted CAM manifest declaring dealer manifests
  /// </summary>
  public interface IManifestFile
  {
    Task<Manifest> Download();
  }
}