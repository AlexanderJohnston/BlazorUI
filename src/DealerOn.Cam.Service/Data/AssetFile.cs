using System.Net.Http;
using System.Threading.Tasks;
using DealerOn.Cam.Data;
using Totem.IO;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// A Toyota-hosted asset file for a campaign
  /// </summary>
  public sealed class AssetFile : IAssetFile
  {
    readonly IHttpClientFactory _http;
    readonly IAssetFolder _folder;

    public AssetFile(IHttpClientFactory http, IAssetFolder folder)
    {
      _http = http;
      _folder = folder;
    }

    public async Task Download(HttpLink link)
    {
      using(var httpClient = _http.CreateClient())
      {
        var response = await httpClient.GetAsync(link.ToString());

        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadAsStreamAsync();

        await _folder.WriteFile(link, data);
      }
    }
  }
}