using System;
using Totem;
using Totem.IO;
using Totem.Runtime;

namespace BlazorUI.Client.Campaign.Data
{
  /// <summary>
  /// A call to <see cref="ICampaignDb"/> to import dealer campaigns and record errors
  /// </summary>
  [Durable]
  public class CampaignDbCall
  {
    public CampaignDbCall(Many<Dealer> dealers, Many<Id> unenrolledDealerIds)
    {
      Dealers = dealers;
      UnenrolledDealerIds = unenrolledDealerIds;
    }

    public readonly Many<Dealer> Dealers;
    public readonly Many<Id> UnenrolledDealerIds;
    public readonly Many<Error> Errors = new Many<Error>();

    public void AddError(Id dealerId, Exception error) =>
      Errors.Write.Add(new Error(dealerId, error.ToString()));

    public class Dealer
    {
      public Dealer(Id id, Many<Campaign> campaigns)
      {
        Id = id;
        Campaigns = campaigns;
      }

      public readonly Id Id;
      public readonly Many<Campaign> Campaigns;
    }

    public class Campaign
    {
      public Campaign(
        FileName assetName,
        bool assetFailed,
        DealerManifest.Page page,
        int priority,
        bool required,
        HttpLink link,
        string altText,
        DateTime whenStarts,
        DateTime whenEnds,
        string model,
        string modelYear)
      {
        AssetName = assetName;
        AssetFailed = assetFailed;
        Page = page;
        Priority = priority;
        Required = required;
        Link = link;
        AltText = altText;
        WhenStarts = whenStarts;
        WhenEnds = whenEnds;
        Model = model;
        ModelYear = modelYear;
      }

      public readonly FileName AssetName;
      public readonly bool AssetFailed;
      public readonly DealerManifest.Page Page;
      public readonly int Priority;
      public readonly bool Required;
      public readonly HttpLink Link;
      public readonly string AltText;
      public readonly DateTime WhenStarts;
      public readonly DateTime WhenEnds;
      public readonly string Model;
      public readonly string ModelYear;
    }

    public class Error
    {
      public Error(Id dealerId, string message)
      {
        DealerId = dealerId;
        Message = message;
      }

      public readonly Id DealerId;
      public readonly string Message;
    }
  }
}