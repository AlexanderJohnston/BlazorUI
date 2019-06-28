using Totem.IO;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// The path shared by all CAM banners
  /// </summary>
  public sealed class BannerPath : IBannerPath
  {
    readonly string _basePath;

    public BannerPath(string basePath)
    {
      _basePath = basePath;
    }

    public string GetPath(FileName fileName)
    {
      var path = _basePath;

      if(!path.EndsWith("/"))
      {
        path += "/";
      }

      return path + fileName.ToString();
    }
  }
}