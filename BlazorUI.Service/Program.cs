using BlazorUI.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Totem.App.Service;

namespace BlazorUI.Service
{
    class Program
    {
        public static Task Main(string[] args)
        {
            var configuration = new ConfigureServiceApp();
            return ServiceApp.Run<ApplicationArea>(
                configuration.Host(build => {
                    build.ConfigureAppConfiguration((context, configuration) =>
                    {
                        configuration.AddUserSecrets<LegacyDatabase>();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        services.AddApplication();
                    });
                })
            );
        }
    }
}
