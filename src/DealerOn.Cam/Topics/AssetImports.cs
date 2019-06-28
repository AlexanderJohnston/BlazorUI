using System.Collections.Generic;
using Totem;
using Totem.IO;
using Totem.Timeline;

namespace DealerOn.Cam.Topics
{
  /// <summary>
  /// The timeline of importing CAM assets into the filesystem and DealerOn database
  /// </summary>
  public class AssetImports : Topic
  {
    HashSet<Id> _pendingAssetIds = new HashSet<Id>();
    HttpLink _assetsLink;

    void Given(ManifestDownloaded e) =>
      _assetsLink = e.AssetsLink;

    void Given(DealerManifestDownloaded e)
    {
      foreach(var asset in e.Assets)
      {
        _pendingAssetIds.Add(asset.Id);
      }
    }

    void Given(AssetImported e) =>
      _pendingAssetIds.Remove(e.AssetId);

    void Given(AssetImportFailed e) =>
      _pendingAssetIds.Remove(e.AssetId);

    //
    // When
    //

    void When(DealerManifestsDownloaded e)
    {
      if(_pendingAssetIds.Count == 0)
      {
        Then(new AssetsImported());
      }
      else
      {
        Then(new AssetImportsStarted(_assetsLink, _pendingAssetIds.ToMany()));
      }
    }

    void When(AssetImported e) =>
      CheckFinished();

    void When(AssetImportFailed e) =>
      CheckFinished();

    void CheckFinished()
    {
      if(_pendingAssetIds.Count == 0)
      {
        Then(new AssetsImported());
      }
    }
  }
}