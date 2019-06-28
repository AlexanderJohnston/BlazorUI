using System.Linq;
using DealerOn.Cam.Data;
using Totem;
using Totem.IO;
using Totem.Timeline;

namespace DealerOn.Cam.Queries
{
  /// <summary>
  /// Maintains a data structure describing a dealer's details
  /// </summary>
  public class DealerDetailsQuery : Query
  {
    static Many<Id> RouteFirst(ManifestDownloaded e) =>
      e.AddedDealers
      .Concat(e.UpdatedDealers)
      .Select(dealer => dealer.Id)
      .Concat(e.RemovedDealerIds)
      .ToMany();
    static Id RouteFirst(EnrollmentChanged e) => e.DealerId;
    static Id Route(DealerManifestDownloaded e) => e.DealerId;
    static Id Route(DealerManifestDownloadFailed e) => e.DealerId;
    static Many<Id> Route(CampaignsImported e) => e.Errors.ToMany(error => error.DealerId);

    public Id DealerId;
    public string Code;
    public string Name;
    public string Region;
    public string Hostname;
    public HttpLink ManifestLink;
    public bool EnrolledOnHome;
    public bool EnrolledOnConditional;
    public string ManifestDownloadError;
    public string CampaignImportError;

    void Given(ManifestDownloaded e)
    {
      if(e.RemovedDealerIds.Contains(Id))
      {
        ThenDone();
      }
      else
      {
        DealerId = Id;

        var dealer = e.AddedDealers
          .Concat(e.UpdatedDealers)
          .Where(d => d.Id == Id)
          .First();

        Code = dealer.Code;
        Name = dealer.Name;
        Region = dealer.Region;
        Hostname = dealer.Hostname;
        ManifestLink = dealer.ManifestLink;
      }
    }

    void Given(EnrollmentChanged e)
    {
      EnrolledOnHome = e.Pages.HasFlag(Pages.Home);
      EnrolledOnConditional = e.Pages.HasFlag(Pages.Conditional);
    }

    void Given(DealerManifestDownloaded e)
    {
      ManifestDownloadError = null;
      CampaignImportError = null;
    }

    void Given(DealerManifestDownloadFailed e) =>
      ManifestDownloadError = e.Error;

    void Given(CampaignsImported e) =>
      CampaignImportError = e.Errors.First(error => error.DealerId == Id).Message;
  }
}