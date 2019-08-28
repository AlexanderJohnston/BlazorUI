using System.Net.Http;
using DealerOn.Cam.Data;
using DealerOn.Cam.Service.Data;
using DealerOn.Cam.Service.Data.Banners;
using Microsoft.Extensions.DependencyInjection;
using Totem.IO;
using Totem.Runtime.Hosting;

namespace DealerOn.Cam.Service
{
  /// <summary>
  /// Extends <see cref="IServiceCollection"/> to declare the CAM runtime
  /// </summary>
  public static class CamServiceExtensions
  {
    public static IServiceCollection AddCam(this IServiceCollection services) =>
      services
      .AddHttpClient()
      .AddCamOptions()
      .AddDealerOnDb()
      .AddManifests()
      .AddAssets()
      .AddCampaigns();

    static IServiceCollection AddCamOptions(this IServiceCollection services) =>
      services.BindOptionsToConfiguration<CamOptions>("cam");

    static IServiceCollection AddDealerOnDb(this IServiceCollection services) =>
      services.AddSingleton<IDealerOnDb>(s => new DealerOnDb(s.GetOptions<CamOptions>().DealerOnConnectionString));

    static IServiceCollection AddManifests(this IServiceCollection services) =>
      services
      .AddSingleton<IManifestFile>(s => new ManifestFile(
        HttpLink.From(s.GetOptions<CamOptions>().ManifestLink),
        s.GetRequiredService<IHttpClientFactory>(),
        s.GetRequiredService<IEligibleDealerDb>()))
      .AddSingleton<IEligibleDealerDb, EligibleDealerDb>()
      .AddSingleton<IDealerManifestFile, DealerManifestFile>();

    static IServiceCollection AddAssets(this IServiceCollection services) =>
      services
      .AddSingleton<IAssetFile, AssetFile>()
      .AddSingleton<IAssetFolder>(s => new AssetFolder(s.GetOptions<CamOptions>().AssetFolder))
      .AddSingleton<IAssetDb, AssetDb>();

    static IServiceCollection AddCampaigns(this IServiceCollection services) =>
      services
      .AddSingleton<IBannerPath>(s => new BannerPath(s.GetOptions<CamOptions>().BannerPath))
      .AddSingleton<HomeBannerTable>()
      .AddSingleton<ConditionalBannerTable>()
      .AddSingleton<ICampaignDb, CampaignDb>(s => new CampaignDb(
        s.GetRequiredService<IDealerOnDb>(),
        s.GetRequiredService<HomeBannerTable>(),
        s.GetRequiredService<ConditionalBannerTable>()));
  }
}