using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Totem.IO;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// The DealerOn folder containing downloaded assets
  /// </summary>
  public sealed class AssetFolder : IAssetFolder
  {
    readonly string _path;

    public AssetFolder(string path)
    {
      _path = path;
    }

    public async Task WriteFile(HttpLink link, Stream data)
    {
      EnsurePathCreated();

      using(var file = OpenFile(link))
      {
        await data.CopyToAsync(file);
      }
    }

    void EnsurePathCreated() =>
      Directory.CreateDirectory(_path);

    FileStream OpenFile(HttpLink link)
    {
      var name = link.Resource.Path.Segments.Last().ToString();

      var path = Path.Combine(_path, name);

      return File.Open(path, FileMode.Create, FileAccess.Write);
    }
  }
}