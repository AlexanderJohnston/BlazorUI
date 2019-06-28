using System.Threading.Tasks;

namespace DealerOn.Cam.Data
{
  /// <summary>
  /// Describes the Toyota-hosted CAM manifest declaring dealer manifests
  /// </summary>
  public interface IManifestFile
  {
    Task<Manifest> Download();
  }
}