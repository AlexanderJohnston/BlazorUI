using System;
using Totem;
using Totem.IO;
using Totem.Runtime;

namespace BlazorUI.Client.Campaign.Data
{
  /// <summary>
  /// The contents of a CAM manifest declaring dealer campaigns
  /// </summary>
  [Durable]
  public class DealerManifest
  {
    public DealerManifest(Many<Asset> assets, Many<Campaign> campaigns)
    {
      Assets = assets;
      Campaigns = campaigns;
    }

    public readonly Many<Asset> Assets;
    public readonly Many<Campaign> Campaigns;

    public class Asset
    {
      public Asset(Id id, FileResource file, string caption)
      {
        Id = id;
        File = file;
        Caption = caption;
      }

      public readonly Id Id;
      public readonly FileResource File;
      public readonly string Caption;
    }

    public class Campaign
    {
      public Campaign(
        Id id,
        Id assetId,
        Page page,
        int priority,
        bool required,
        HttpLink link,
        string altText,
        DateTime whenStarts,
        DateTime whenEnds,
        string model,
        string modelYear)
      {
        Id = id;
        AssetId = assetId;
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

      public readonly Id Id;
      public readonly Id AssetId;
      public readonly Page Page;
      public readonly int Priority;
      public readonly bool Required;
      public readonly HttpLink Link;
      public readonly string AltText;
      public readonly DateTime WhenStarts;
      public readonly DateTime WhenEnds;
      public readonly string Model;
      public readonly string ModelYear;
    }

    public enum Page
    {
      Home,
      Srp,
      Vdp,
      SrpVdp
    }
  }
}