using System.Threading.Tasks;

namespace BlazorUI.Client.Campaign.Data
{
  /// <summary>
  /// Describes the database containing campaign assets
  /// </summary>
  public interface IAssetDb
  {
    Task Merge(DealerManifest.Asset asset);
  }
}