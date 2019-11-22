using BlazorUI.Service.Metrics;
using BlazorUI.Service.Models;
using BlazorUI.Shared;
using BlazorUI.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Totem.App.Service;

namespace BlazorUI.Service
{
    class Program
    {
        //public static Task Main(string[] args) => ServiceApp.Run<ApplicationArea>(services => services.AddApplication());

        public static Task Main(string[] args)
        {
            var configuration = new ConfigureServiceApp();
            return ServiceApp.Run<ApplicationArea>(
                configuration.Host(build =>
                {
                    build.ConfigureAppConfiguration((context, configuration) =>
                    {
                        configuration.AddUserSecrets<LegacyEvents>();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        var legacy = context.Configuration.GetSection("LegacyEvents:ConnectionString");
                        services.AddApplicationConfigured(legacy.Value);
                        //services.AddHostedService<PostsharpBackendLogging>();
                        services.AddHostedService(s => new MetricsService(new MetricsClient(), TimeSpan.FromMilliseconds(100)));
                    });
                })
            );
        }
    }
}
