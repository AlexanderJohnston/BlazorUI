    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerOn.Cam.Data;
using Totem;
using Totem.Timeline;
using static Totem.ManyExtensions;

namespace DealerOn.Cam.Topics
{
  /// <summary>
  /// The timeline of importing a dealer's data from the CAM program
  /// </summary>
  public class DealerManifestDownload : Topic
  {
    static Many<Id> RouteFirst(ManifestDownloaded e) =>
      e.AddedDealers
      .Concat(e.UpdatedDealers)
      .Select(dealer => dealer.Id)
      .Concat(e.RemovedDealerIds)
      .ToMany();

    static Id Route(EnrollmentChanged e) => e.DealerId;
    static Many<Id> Route(DealerManifestDownloadsStarted e) => e.DealerIds;
    static Id Route(DealerManifestDownloaded e) => e.DealerId;

    readonly Dictionary<Id, DealerManifest.Campaign> _campaignsById = new Dictionary<Id, DealerManifest.Campaign>();
    Manifest.Dealer _dealer;
    Pages _pages;

    void Given(ManifestDownloaded e) =>
      _dealer = e.AddedDealers
        .Concat(e.UpdatedDealers)
        .Where(dealer => dealer.Id == Id)
        .FirstOrDefault();

    void Given(EnrollmentChanged e)
    {
      _pages = e.Pages;

      if(e.Pages == Pages.None)
      {
        _campaignsById.Clear();
      }
    }

    void Given(DealerManifestDownloaded e)
    {
      foreach(var added in e.AddedCampaigns)
      {
        _campaignsById.Add(added.Id, added);
      }

      foreach(var updated in e.UpdatedCampaigns)
      {
        _campaignsById[updated.Id] = updated;
      }

      foreach(var removedId in e.RemovedCampaignIds)
      {
        _campaignsById.Remove(removedId);
      }
    }

    //
    // When
    //

    void When(ManifestDownloaded e)
    {
      if(e.RemovedDealerIds.Contains(Id))
      {
        ThenDone();
      }
    }

    async Task When(DealerManifestDownloadsStarted e, IDealerManifestFile file)
    {
      try
      {
        var manifest = await file.Download(_dealer, _pages);

        var added = new Many<DealerManifest.Campaign>();
        var updated = new Many<DealerManifest.Campaign>();
        //var remainingIds = _campaignsById.Keys.ToHashSet();
        var remainingIds = ManyExtensions.ToHashSet(_campaignsById.Keys);

        foreach(var downloaded in manifest.Campaigns)
        {
          remainingIds.Remove(downloaded.Id);

          if(!_campaignsById.TryGetValue(downloaded.Id, out var existing))
          {
            added.Write.Add(downloaded);
          }
          else
          {
            if(downloaded.AssetId != existing.AssetId
              || downloaded.Page != existing.Page
              || downloaded.Priority != existing.Priority
              || downloaded.Required != existing.Required
              || downloaded.Link != existing.Link
              || downloaded.AltText != existing.AltText
              || downloaded.WhenStarts != existing.WhenStarts
              || downloaded.WhenEnds != existing.WhenEnds
              || downloaded.Model != existing.Model
              || downloaded.ModelYear != existing.ModelYear)
            {
              updated.Write.Add(downloaded);
            }
          }
        }

        Then(new DealerManifestDownloaded(Id, manifest.Assets, added, updated, remainingIds.ToMany()));
      }
      catch(Exception error)
      {
        Then(new DealerManifestDownloadFailed(Id, error.ToString()));
      }
    }
  }
}