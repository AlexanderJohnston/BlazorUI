using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorUI.Client.Campaign.Data;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Client.Campaign.Topics
{
  /// <summary>
  /// The timeline of downloading the CAM manifest from the Toyota CDN
  /// </summary>
  public class ManifestDownload : Topic
  {
    readonly Dictionary<Id, Manifest.Dealer> _dealersById = new Dictionary<Id, Manifest.Dealer>();

    void Given(ManifestDownloaded e)
    {
      foreach(var added in e.AddedDealers)
      {
        _dealersById[added.Id] = added;
      }

      foreach(var updated in e.UpdatedDealers)
      {
        _dealersById[updated.Id] = updated;
      }

      foreach(var removed in e.RemovedDealerIds)
      {
        _dealersById.Remove(removed);
      }
    }

    //
    // When
    //

    async Task When(ImportStarted e, IManifestFile file)
    {
      try
      {
        var manifest = await file.Download();

        var added = new Many<Manifest.Dealer>();
        var updated = new Many<Manifest.Dealer>();
        var remainingIds = _dealersById.Keys.ToHashSet();

        foreach(var downloaded in manifest.Dealers)
        {
          remainingIds.Remove(downloaded.Id);

          if(!_dealersById.TryGetValue(downloaded.Id, out var existing))
          {
            added.Write.Add(downloaded);
          }
          else
          {
            if(downloaded.Name != existing.Name
              || downloaded.Region != existing.Region
              || downloaded.Hostname != existing.Hostname
              || downloaded.ManifestLink != existing.ManifestLink)
            {
              updated.Write.Add(downloaded);
            }
          }
        }

        Then(new ManifestDownloaded(manifest.AssetsLink, added, updated, remainingIds.ToMany()));
      }
      catch(Exception error)
      {
        Then(new ManifestDownloadFailed(error.ToString()));
      }
    }
  }
}