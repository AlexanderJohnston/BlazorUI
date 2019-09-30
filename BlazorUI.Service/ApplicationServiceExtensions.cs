using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Runtime.Hosting;

namespace BlazorUI.Service
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) => services.AddHttpClient().AddApplicationOptions();

        static IServiceCollection AddApplicationOptions(this IServiceCollection services) => services.BindOptionsToConfiguration<ApplicationOptions>("app");
        /*
        static IServiceCollection AddDealerOnDb(this IServiceCollection services) =>
          services.AddSingleton<TorqueQAContext>(s => new TorqueQAContext(s.GetOptions<ApplicationOptions>().DealerOnConnectionString));/*
        /*
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
            */
    }
}
