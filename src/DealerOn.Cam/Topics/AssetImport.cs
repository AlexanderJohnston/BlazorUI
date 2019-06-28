using System;
using System.Linq;
using System.Threading.Tasks;
using DealerOn.Cam.Data;
using Totem;
using Totem.IO;
using Totem.Timeline;

namespace DealerOn.Cam.Topics
{
  /// <summary>
  /// The timeline of downloading a single CAM asset file
  /// </summary>
  public class AssetImport : Topic
  {
    static Many<Id> RouteFirst(DealerManifestDownloaded e) => e.Assets.ToMany(asset => asset.Id);
    static Many<Id> Route(AssetImportsStarted e) => e.AssetIds;

    DealerManifest.Asset _asset;

    void Given(DealerManifestDownloaded e) =>
      _asset = e.Assets.First(asset => asset.Id == Id);

    //
    // When
    //

    async Task When(AssetImportsStarted e, IAssetFile file, IAssetDb db)
    {
      try
      {
        var link = e.Link.Then(HttpResource.From(_asset.File));

        await file.Download(link);

        await db.Merge(_asset);

        Then(new AssetImported(Id));
      }
      catch(Exception error)
      {
        Then(new AssetImportFailed(Id, error.ToString()));
      }
    }
  }
}