using System.Collections.Generic;
using Totem;
using Totem.Timeline;

namespace DealerOn.Cam.Topics
{
  /// <summary>
  /// The timeline of downloading and parsing CAM dealer manifests
  /// </summary>
  public class DealerManifestDownloads : Topic
  {
    HashSet<Id> _pendingDealerIds = new HashSet<Id>();

    void Given(DealerManifestDownloadsStarted e) =>
      _pendingDealerIds.AddRange(e.DealerIds);

    void Given(DealerManifestDownloaded e) =>
      _pendingDealerIds.Remove(e.DealerId);

    void Given(DealerManifestDownloadFailed e) =>
      _pendingDealerIds.Remove(e.DealerId);

    //
    // When
    //

    void When(DealerManifestDownloaded e) =>
      CheckFinished();

    void When(DealerManifestDownloadFailed e) =>
      CheckFinished();

    void CheckFinished()
    {
      if(_pendingDealerIds.Count == 0)
      {
        Then(new DealerManifestsDownloaded());
      }
    }
  }
}