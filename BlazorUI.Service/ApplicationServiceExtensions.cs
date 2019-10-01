using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Totem.Runtime.Hosting;
using BlazorUI.Service.Models;
using BlazorUI.Shared.Data;
using BlazorUI.Service.Data;

namespace BlazorUI.Service
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationConfigured(this IServiceCollection services, string connection) => 
            services.AddApplication().AddDatabaseWithSecrets(connection);

        public static IServiceCollection AddApplicationHardCoded(this IServiceCollection services) =>
            services.AddApplication().AddLegacyEventDatabase();

        static IServiceCollection AddApplication(this IServiceCollection services) =>
            services
            .AddHttpClient()
            .AddApplicationOptions();

        static IServiceCollection AddApplicationOptions(this IServiceCollection services) => 
            services.BindOptionsToConfiguration<ApplicationOptions>("app");

        /// <summary>
        ///     Legacy method of attaching the database. Hard-coded.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        static IServiceCollection AddLegacyEventDatabase(this IServiceCollection services) => 
            services.AddSingleton<ILegacyEventContext, TorqueQAContext>(s => new TorqueQAContext("connectionString"));

        /// <summary>
        ///     User secrets method of attaching the database.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        static IServiceCollection AddDatabaseWithSecrets(this IServiceCollection services, string connection)
        {
            return services.AddSingleton<ILegacyEventContext, DatabaseService>(s => new DatabaseService(connection));
        }

        /*
        static IServiceCollection AddDealerOnDb(this IServiceCollection services) =>
          services.AddSingleton<IDealerOnDb>(s => new DealerOnDb(s.GetOptions<ApplicationOptions>().DealerOnConnectionString));

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
