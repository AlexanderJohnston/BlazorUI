using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using DealerOn.Cam.Data;
using Totem;
using Totem.IO;
using Totem.Runtime;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// A Toyota-hosted CAM manifest declaring a dealer's campaigns
  /// </summary>
  public sealed class DealerManifestFile : IDealerManifestFile
  {
    readonly IHttpClientFactory _http;

    public DealerManifestFile(IHttpClientFactory http)
    {
      _http = http;
    }

    public async Task<DealerManifest> Download(Manifest.Dealer dealer, Pages pages)
    {
      using(var httpClient = _http.CreateClient())
      {
        var response = await httpClient.GetAsync(dealer.ManifestLink.ToString());

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadAsStreamAsync();

        var xml = XDocument.Load(data).Root;

        return new DealerManifestBuilder(dealer, xml, pages).Build();
      }
    }

    class DealerManifestBuilder : Notion
    {
      const int _homeWidth = 880;
      const int _homeHeight = 320;
      const int _conditionalWidth = 1650;
      const int _conditionalHeight = 150;

      readonly Many<DealerManifest.Asset> _assets = new Many<DealerManifest.Asset>();
      readonly Many<DealerManifest.Campaign> _campaigns = new Many<DealerManifest.Campaign>();
      readonly Manifest.Dealer _dealer;
      readonly XElement _xml;
      readonly bool _homeEnabled;
      readonly bool _conditionalEnabled;
      int _priority;

      public DealerManifestBuilder(Manifest.Dealer dealer, XElement xml, Pages pages)
      {
        _dealer = dealer;
        _xml = xml;

        _homeEnabled = pages.HasFlag(Pages.Home);
        _conditionalEnabled = pages.HasFlag(Pages.Conditional);
      }

      internal DealerManifest Build()
      {
        BuildAssetsAndCampaigns();

        return new DealerManifest(_assets, _campaigns);
      }

      void BuildAssetsAndCampaigns()
      {
        var campaigns =
          from campaignXml in _xml.Element("CAMCampaigns").Elements("CAMCampaign")
          let creativeXml = campaignXml.Element("CAMCreative")
          select new
          {
            CampaignXml = campaignXml,
            CreativeXml = creativeXml,
            Required = (bool) creativeXml.Attribute("Required"),
            National = ((string) campaignXml.Attribute("groupName")).Equals("National", StringComparison.OrdinalIgnoreCase),
            Priority = (int) campaignXml.Attribute("priority")
          };

        if(RegionIs("cat") || RegionIs("new york"))
        {
          campaigns = campaigns
            .OrderByDescending(campaign => campaign.Required)
            .ThenBy(campaign => campaign.National)
            .ThenBy(campaign => campaign.Priority);
        }
        else
        {
          campaigns = campaigns
            .OrderByDescending(campaign => campaign.Required)
            .ThenByDescending(campaign => campaign.National)
            .ThenBy(campaign => campaign.Priority);
        }

        foreach(var campaign in campaigns)
        {
          if(TryGetEnabledPage(campaign.CreativeXml, out var page))
          {
            BuildCampaign(campaign.CampaignXml, campaign.CreativeXml, page);
          }
        }
      }

      bool RegionIs(string value) =>
        _dealer.Region.Equals(value, StringComparison.OrdinalIgnoreCase);

      bool TryGetEnabledPage(XElement creativeXml, out DealerManifest.Page page)
      {
        var xmlPage = GetPageOrNull(creativeXml);

        var isHome = _homeEnabled && xmlPage == DealerManifest.Page.Home;
        var isConditional = _conditionalEnabled && xmlPage != null && xmlPage != DealerManifest.Page.Home;

        if(isHome || isConditional)
        {
          page = xmlPage.Value;

          return true;
        }

        page = default;

        return false;
      }

      DealerManifest.Page? GetPageOrNull(XElement creativeXml)
      {
        switch((string) creativeXml.Attribute("LandingPageType"))
        {
          case "Home Page": return DealerManifest.Page.Home;
          case "SRP": return DealerManifest.Page.Srp;
          case "VDP": return DealerManifest.Page.Vdp;
          case "SRP/VDP": return DealerManifest.Page.SrpVdp;
          default: return null;
        }
      }

      void BuildCampaign(XElement campaignXml, XElement creativeXml, DealerManifest.Page page)
      {
        var pageWidth = page == DealerManifest.Page.Home ? _homeWidth : _conditionalWidth;
        var pageHeight = page == DealerManifest.Page.Home ? _homeHeight : _conditionalHeight;

        var assets =
          from sourceXml in creativeXml.Elements("CAMCreativeSource")
          from assetXml in sourceXml.Elements("CAMAsset")
          where (int) assetXml.Attribute("Width") == pageWidth
          where (int) assetXml.Attribute("Height") == pageHeight
          let id = (string) assetXml.Attribute("Id")
          let path = FixPercentSigns((string) assetXml.Attribute("Path"))
          let name = FixPercentSigns((string) assetXml.Attribute("Name"))
          let caption = (string) campaignXml.Attribute("name")
          select new DealerManifest.Asset(Id.From(id), FileResource.From(path, name), caption);

        var asset = assets.FirstOrDefault();

        if(asset == null)
        {
          Log.Warning("Dealer {DealerId} has a campaign missing asset size {Width}x{Height}\n\n{ManifestLink}\n\n{Xml}", _dealer.Id, pageWidth, pageHeight, _dealer.ManifestLink, campaignXml);
        }
        else
        {
          _assets.Write.Add(asset);

          _campaigns.Write.Add(new DealerManifest.Campaign(
            Id.From((string) campaignXml.Attribute("id")),
            asset.Id,
            page,
            _priority,
            (bool) campaignXml.Attribute("required"),
            ExpandLink(creativeXml),
            TruncateAltText((string) creativeXml.Attribute("ImageAltText")),
            (DateTime) campaignXml.Attribute("startDate"),
            (DateTime) campaignXml.Attribute("endDate"),
            (string) creativeXml.Attribute("Model"),
            (string) creativeXml.Attribute("ModelYear")));

          _priority++;
        }
      }

      string FixPercentSigns(string path) =>
        Regex.Replace(path, "%(?!([0-9A-F]{2}))", "%25");

      string TruncateAltText(string altText) =>
        altText.Length <= 100 ? altText : altText.Substring(0, 97) + "...";

      HttpLink ExpandLink(XElement creativeXml)
      {
        var link = HttpLink.From(HttpHost.FromHttp(_dealer.Hostname));

        var resource = GetLinkResource(
          (string) creativeXml.Attribute("Link"),
          (string) creativeXml.Attribute("Model") ?? "");

        if(resource != null)
        {
          link = link.Then(HttpResource.From(resource));
        }

        return link;
      }

      string GetLinkResource(string link, string model)
      {
        switch(link.ToLower())
        {
          case "new inventory": return AppendModel("searchnew.aspx", model);
          case "show room": return "showroom.aspx";
          case "tcuv": return AppendModel("searchused.aspx?cpo=1", model);
          case "specials": return "specials.aspx";
          case "service": return "service.aspx";
          case "parts": return "parts.html";
          case "college grad rebates": return "college-grad.html";
          case "military rebates": return "military-rebate.html";
          case "about": return "aboutus.aspx";
          case "directions": return "hours.aspx";
          case "reviews": return "customer-reviews.html";
          case "awards": return "";
          case "testimonials": return "customer-reviews.html";
          case "new incentives": return "Toyota-Incentives.html";
          case "trac": return "Toyota-rent-a-car";
          case "toyotacare": return "ToyotaCare.html";
          case "toyota express maintenance": return "service-center-express-maintenance.html";
          default: return null;
        }
      }

      string AppendModel(string baseResource, string model)
      {
        switch(model.ToLower())
        {
          case "all models":
          case "na":
            return baseResource;
          default:
            baseResource += baseResource.Contains('?') ? "&" : "?";

            return baseResource + "model=" + model;
        }
      }
    }
  }
}