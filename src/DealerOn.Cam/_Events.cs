using DealerOn.Cam.Data;
using Totem;
using Totem.IO;
using Totem.Timeline;

namespace DealerOn.Cam
{
  //
  // Enrollment
  //

  public class DealerNotEnrolled : Event
  {
    public DealerNotEnrolled(Id dealerId)
    {
      DealerId = dealerId;
    }

    public readonly Id DealerId;
  }

  public class ChangeEnrollment : Command
  {
    public ChangeEnrollment(Id dealerId, Pages pages)
    {
      DealerId = dealerId;
      Pages = pages;
    }

    public readonly Id DealerId;
    public readonly Pages Pages;
  }

  public class EnrollmentChanged : Event
  {
    public EnrollmentChanged(Id dealerId, Pages pages)
    {
      DealerId = dealerId;
      Pages = pages;
    }

    public readonly Id DealerId;
    public readonly Pages Pages;
  }

  public class EnrollmentUnchanged : Event
  {
    public EnrollmentUnchanged(Id dealerId)
    {
      DealerId = dealerId;
    }

    public readonly Id DealerId;
  }

  //
  // Import
  //

  public class StartImport : Command
  {}

  public class StartScheduledImport : Command
  {}

  public class ImportStarted : Event
  {}

  public class ImportAlreadyStarted : Event
  {}

  public class ImportFinished : Event
  {}

  //
  // Manifest downloads
  //

  public class ManifestDownloaded : Event
  {
    public ManifestDownloaded(
      HttpLink assetsLink,
      Many<Manifest.Dealer> addedDealers,
      Many<Manifest.Dealer> updatedDealers,
      Many<Id> removedDealerIds)
    {
      AssetsLink = assetsLink;
      AddedDealers = addedDealers;
      UpdatedDealers = updatedDealers;
      RemovedDealerIds = removedDealerIds;
    }

    public readonly HttpLink AssetsLink;
    public readonly Many<Manifest.Dealer> AddedDealers;
    public readonly Many<Manifest.Dealer> UpdatedDealers;
    public readonly Many<Id> RemovedDealerIds;
  }

  public class ManifestDownloadFailed : ErrorEvent
  {
    public ManifestDownloadFailed(string error) : base(error)
    {}
  }

  //
  // Dealer manifest downloads
  //

  public class DealerManifestDownloadsStarted : Event
  {
    public DealerManifestDownloadsStarted(Many<Id> dealerIds)
    {
      DealerIds = dealerIds;
    }

    public readonly Many<Id> DealerIds;
  }

  public class DealerManifestDownloaded : Event
  {
    public DealerManifestDownloaded(
      Id dealerId,
      Many<DealerManifest.Asset> assets,
      Many<DealerManifest.Campaign> addedCampaigns,
      Many<DealerManifest.Campaign> updatedCampaigns,
      Many<Id> removedCampaignIds)
    {
      DealerId = dealerId;
      Assets = assets;
      AddedCampaigns = addedCampaigns;
      UpdatedCampaigns = updatedCampaigns;
      RemovedCampaignIds = removedCampaignIds;
    }

    public readonly Id DealerId;
    public readonly Many<DealerManifest.Asset> Assets;
    public readonly Many<DealerManifest.Campaign> AddedCampaigns;
    public readonly Many<DealerManifest.Campaign> UpdatedCampaigns;
    public readonly Many<Id> RemovedCampaignIds;
  }

  public class DealerManifestDownloadFailed : ErrorEvent
  {
    public DealerManifestDownloadFailed(Id dealerId, string error) : base(error)
    {
      DealerId = dealerId;
    }

    public readonly Id DealerId;
  }

  public class DealerManifestDownloadsSkipped : Event
  {}

  public class DealerManifestsDownloaded : Event
  {}

  //
  // Asset imports
  //

  public class AssetImportsStarted : Event
  {
    public AssetImportsStarted(HttpLink link, Many<Id> assetIds)
    {
      Link = link;
      AssetIds = assetIds;
    }

    public readonly HttpLink Link;
    public readonly Many<Id> AssetIds;
  }

    public class AssetImported : Event
  {
    public AssetImported(Id assetId)
    {
      AssetId = assetId;
    }

    public readonly Id AssetId;
  }

  public class AssetImportFailed : ErrorEvent
  {
    public AssetImportFailed(Id assetId, string error) : base(error)
    {
      AssetId = assetId;
    }

    public readonly Id AssetId;
  }

  public class AssetsImported : Event
  {}

  //
  // Campaign imports
  //

  public class CampaignsImported : Event
  {
    public CampaignsImported(Many<CampaignDbCall.Error> errors)
    {
      Errors = errors;
    }

    public readonly Many<CampaignDbCall.Error> Errors;
  }

  public class CampaignImportFailed : ErrorEvent
  {
    public CampaignImportFailed(string error) : base(error)
    {}
  }
}