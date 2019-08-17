using System;
using System.Linq;
using BlazorUI.Client.Campaign.Data;
using Totem;
using Totem.Timeline;

namespace BlazorUI.Client.Campaign.Queries
{
  /// <summary>
  /// A data structure describing the set of CAM regions
  /// </summary>
  public class RegionsQuery : Query
  {
    public Many<Region> Regions = new Many<Region>();

    public class Region
    {
      public string Name;
      public Many<Dealer> Dealers = new Many<Dealer>();
    }

    public class Dealer
    {
      public Id DealerId;
      public string Name;
      public string DetailsLink;
      public bool IsEnrolled;
    }
    void Given(FakeManifest e)
    {

    }
    void Given(EnrollmentChanged e) =>
      Regions
      .SelectMany(region => region.Dealers)
      .Where(dealer => dealer.DealerId == e.DealerId)
      .First()
      .IsEnrolled = e.Pages != Data.Pages.None;

    void Given(ManifestDownloaded e)
    {
      var dealers =
        from region in Regions
        from dealer in region.Dealers
        select new
        {
          dealer.DealerId,
          Region = region.Name,
          dealer.Name,
          dealer.DetailsLink,
          dealer.IsEnrolled
        };

      var dealersById = dealers.ToDictionary(dealer => dealer.DealerId);

      foreach(var added in e.AddedDealers)
      {
        dealersById.Add(added.Id, new
        {
          DealerId = added.Id,
          added.Region,
          added.Name,
          DetailsLink = $"/api/dealers/{added.Id}",
          IsEnrolled = false
        });
      }

      foreach(var updated in e.UpdatedDealers)
      {
        var dealer = dealersById[updated.Id];

        dealersById[updated.Id] = new
        {
          DealerId = updated.Id,
          updated.Region,
          updated.Name,
          dealer.DetailsLink,
          dealer.IsEnrolled
        };
      }

      foreach(var removedId in e.RemovedDealerIds)
      {
        dealersById.Remove(removedId);
      }

      Regions = Many.Of(
        from dealer in dealersById.Values
        group dealer by dealer.Region into regionDealers
        orderby regionDealers.Key
        select new Region
        {
          Name = regionDealers.Key,
          Dealers = Many.Of(
            from dealer in regionDealers
            orderby Int32.Parse(dealer.DealerId.ToString())
            select new Dealer
            {
              DealerId = dealer.DealerId,
              Name = dealer.Name,
              DetailsLink = dealer.DetailsLink,
              IsEnrolled = dealer.IsEnrolled
            })
        });
    }
  }
}