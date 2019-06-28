namespace DealerOn.Cam.Service
{
  /// <summary>
  /// Configures the CAM timeline
  /// </summary>
  public class CamOptions
  {
    public string ManifestLink { get; set; }
    public string AssetFolder { get; set; }
    public string BannerPath { get; set; }
    public string DealerOnConnectionString { get; set; }
  }
}