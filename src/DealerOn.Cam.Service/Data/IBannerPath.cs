using Totem.IO;

namespace DealerOn.Cam.Service.Data
{
  /// <summary>
  /// Describes the base path of all CAM banners in the database
  /// </summary>
  public interface IBannerPath
  {
    string GetPath(FileName assetName);
  }
}