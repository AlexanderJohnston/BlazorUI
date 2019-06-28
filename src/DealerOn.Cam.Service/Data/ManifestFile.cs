using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using DealerOn.Cam.Data;
using Totem;
using Totem.IO;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// The Toyota-hosted CAM manifest declaring dealer manifests
  /// </summary>
  public sealed class ManifestFile : IManifestFile
  {
    readonly HttpLink _link;
    readonly IHttpClientFactory _http;
    readonly IEligibleDealerDb _eligibleDealerDb;

    public ManifestFile(HttpLink link, IHttpClientFactory http, IEligibleDealerDb eligibleDealerDb)
    {
      _link = link;
      _http = http;
      _eligibleDealerDb = eligibleDealerDb;
    }

    public async Task<Manifest> Download()
    {
      var xml = await DownloadXml();

      return new Manifest(ReadAssetsLink(xml), await ReadDealers(xml));
    }

    async Task<XElement> DownloadXml()
    {
      using(var httpClient = _http.CreateClient())
      {
        var response = await httpClient.GetAsync(_link.ToString());

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadAsStreamAsync();

        return XDocument.Load(data).Root;
      }
    }

    HttpLink ReadAssetsLink(XElement xml)
    {
      var link = (string) xml.Attribute("AssetURL");

      // Yes they told use to do this (DEV-12681)
      link = link.Replace("http://www.toyota.com", "https://ssl.toyota.com");

      return HttpLink.From(link);
    }

    async Task<Many<Manifest.Dealer>> ReadDealers(XElement xml)
    {
      var manifestDealers =
        from dealer in xml.Element("CAMDealerWrapper").Elements()
        select new
        {
          Code = ((string) dealer.Attribute("DealerCode")).PadLeft(5, '0'),
          Link = HttpLink.From((string) dealer.Attribute("ManifestURL"))
        };

      return Many.Of(
        from dbDealer in await _eligibleDealerDb.GetEligibleDealers()
        join manifestDealer in manifestDealers on dbDealer.Code equals manifestDealer.Code
        select new Manifest.Dealer(
          dbDealer.Id,
          manifestDealer.Code,
          dbDealer.Name,
          dbDealer.Region,
          dbDealer.Hostname,
          manifestDealer.Link));
    }
  }
}