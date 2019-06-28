using System.Threading.Tasks;

namespace DealerOn.Cam.Data
{
  /// <summary>
  /// Describes the database containing campaign assets
  /// </summary>
  public interface IAssetDb
  {
    Task Merge(DealerManifest.Asset asset);
  }
}