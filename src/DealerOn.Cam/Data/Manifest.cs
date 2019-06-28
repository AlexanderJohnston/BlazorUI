using Totem;
using Totem.IO;
using Totem.Runtime;

namespace DealerOn.Cam.Data
{
  /// <summary>
  /// The contents of the root CAM manifest
  /// </summary>
  [Durable]
  public class Manifest
  {
    public Manifest(HttpLink assetsLink, Many<Dealer> dealers)
    {
      AssetsLink = assetsLink;
      Dealers = dealers;
    }

    public readonly HttpLink AssetsLink;
    public readonly Many<Dealer> Dealers;

    public class Dealer
    {
      public Dealer(
        Id id,
        string code,
        string name,
        string region,
        string hostname,
        HttpLink manifestLink)
      {
        Id = id;
        Code = code;
        Name = name;
        Region = region;
        Hostname = hostname;
        ManifestLink = manifestLink;
      }

      public readonly Id Id;
      public readonly string Code;
      public readonly string Name;
      public readonly string Region;
      public readonly string Hostname;
      public readonly HttpLink ManifestLink;
    }
  }
}