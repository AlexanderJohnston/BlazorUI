using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Totem.App.Web;
using DealerOn.Cam;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BlazorUI.Client;
using Blazor.Extensions;

namespace BlazorUI.Server
{
    public class Program
    {
        public static Task Main()
        {
            // old method, working method
            //Type type = MethodBase.GetCurrentMethod().DeclaringType;
            //return WebApp.Run<CamArea>(Assembly.GetAssembly(type));
            return WebApp.Run<CamArea>(Configure
                .App(configure =>
                {
                    configure.UseClientSideBlazorFiles<BlazorUI.Client.Startup>();
                })
                .Services((context, services) =>
                {
                    services.AddServerSideBlazor();
                    // Server side Blazor doesn't provide HttpClient by default
                    services.AddScoped<HttpClient>(s =>
                    {
                        // Creating the URI helper needs to wait nutil JS Runtime is initialized, so defer it
                        var uriHelper = s.GetRequiredService<IUriHelper>();
                        return new HttpClient
                        {
                            BaseAddress = new Uri(uriHelper.GetBaseUri())
                        };
                    });
                    services.AddHttpClient();
                    services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddRazorRuntimeCompilation()
                    .AddNewtonsoftJson();
                    services.AddResponseCompression(opts =>
                    {
                        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                            new[] { "application/octet-stream" });
                    });
                    // Register client dependecies.
                    services.AddTransient<HubConnectionBuilder>();
                    services.AddSingleton<QueryController>();
                    services.AddSingleton<AppState>();
                })
            );
        }
    }

}
