using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorUI.Client.Campaign.Data;
using Totem;
using Totem.IO;
using Totem.Timeline;

namespace BlazorUI.Client.Campaign.Topics
{
  /// <summary>
  /// The timeline of importing dealer campaigns into the DealerOn database
  /// </summary>
  public class CampaignImport : Topic
  {
    readonly HashSet<Id> _unenrolledDealerIds = new HashSet<Id>();
    readonly Dictionary<Id, Dictionary<Id, DealerManifest.Campaign>> _campaignsByDealerId = new Dictionary<Id, Dictionary<Id, DealerManifest.Campaign>>();
    readonly Dictionary<Id, FileName> _fileNamesByAssetId = new Dictionary<Id, FileName>();
    readonly HashSet<Id> _failedAssetIds = new HashSet<Id>();

    void Given(ManifestDownloaded e)
    {
      foreach(var removedDealerId in e.RemovedDealerIds)
      {
        _unenrolledDealerIds.Add(removedDealerId);
        _campaignsByDealerId.Remove(removedDealerId);
      }
    }

    void Given(EnrollmentChanged e)
    {
      if(e.Pages == Data.Pages.None)
      {
        _unenrolledDealerIds.AddRange(e.DealerId);
        _campaignsByDealerId.Remove(e.DealerId);
      }
    }

    void Given(DealerManifestDownloaded e)
    {
      foreach(var asset in e.Assets)
      {
        _fileNamesByAssetId[asset.Id] = asset.File.Name;
      }

      if(!_campaignsByDealerId.TryGetValue(e.DealerId, out var campaignsById))
      {
        campaignsById = new Dictionary<Id, DealerManifest.Campaign>();

        _campaignsByDealerId[e.DealerId] = campaignsById;
      }

      foreach(var added in e.AddedCampaigns)
      {
        campaignsById.Add(added.Id, added);
      }

      foreach(var updated in e.UpdatedCampaigns)
      {
        campaignsById[updated.Id] = updated;
      }

      foreach(var removedId in e.RemovedCampaignIds)
      {
        campaignsById.Remove(removedId);
      }
    }

    void Given(AssetImportFailed e) =>
      _failedAssetIds.Add(e.AssetId);

    void Given(CampaignsImported e)
    {
      _unenrolledDealerIds.Clear();
      _fileNamesByAssetId.Clear();
      _failedAssetIds.Clear();
    }

    void Given(CampaignImportFailed e)
    {
      _unenrolledDealerIds.Clear();
      _fileNamesByAssetId.Clear();
      _failedAssetIds.Clear();
    }

    //
    // When
    //

    Task When(AssetsImported e, ICampaignDb db) =>
      ImportCampaigns(db);

    Task When(DealerManifestDownloadsSkipped e, ICampaignDb db) =>
      ImportCampaigns(db);

    async Task ImportCampaigns(ICampaignDb db)
    {
      {
        try
        {
          var call = BuildCall();

          await db.ImportCampaigns(call);

          Then(new CampaignsImported(call.Errors));
        }
        catch(Exception error)
        {
          Then(new CampaignImportFailed(error.ToString()));
        }
      }
    }

    //
    // Details
    //

    CampaignDbCall BuildCall() =>
      new CampaignDbCall(BuildCallDealers(), _unenrolledDealerIds.ToMany());

    Many<CampaignDbCall.Dealer> BuildCallDealers() =>
      _campaignsByDealerId.Keys.ToMany(dealerId =>
      {
        var campaignsById = _campaignsByDealerId[dealerId];

        var campaigns =
          from campaign in campaignsById.Values
          orderby campaign.Priority
          select new CampaignDbCall.Campaign(
            _fileNamesByAssetId[campaign.AssetId],
            _failedAssetIds.Contains(campaign.AssetId),
            campaign.Page,
            campaign.Priority,
            campaign.Required,
            campaign.Link,
            campaign.AltText,
            campaign.WhenStarts,
            campaign.WhenEnds,
            campaign.Model,
            campaign.ModelYear);

        return new CampaignDbCall.Dealer(dealerId, campaigns.ToMany());
      });
  }
}