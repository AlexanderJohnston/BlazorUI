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
using Microsoft.Extensions.Hosting;
using Totem.Timeline.SignalR;

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
                .App(app =>
                {
                    var environment = app.ApplicationServices
                        .GetRequiredService<Microsoft.Extensions.Hosting.IHostEnvironment>();
                    if (environment.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                        app.UseBlazorDebugging();
                    }
                    app.UseClientSideBlazorFiles<BlazorUI.Client.Startup>();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseRouting();
                    app.UseCors();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapRazorPages();
                        endpoints.MapBlazorHub();
                        // TODO: Extension method in Timeline.SignalR? -- relies on future .NET Standard Support
                        endpoints.MapHub<QueryHub>("/hubs/query");
                        endpoints.MapDefaultControllerRoute();
                        endpoints.MapControllerRoute("Imports", "{controller=Imports}/{action=StartImport}");
                        endpoints.MapControllerRoute("ImportTest", "{controller=Imports}/{action=Test}");
                        endpoints.MapFallbackToClientSideBlazor<BlazorUI.Client.Startup>("index.html");
                    });
                })
                .Services((context, services) =>
                {
                    services.AddServerSideBlazor();
                    // Server side Blazor doesn't provide HttpClient by default
                    // Should probably be Scoped (to the connection) but Blazor only understands Transient currently.
                    services.AddTransient<HttpClient>(s =>
                    {
                        // Creating the URI helper needs to wait nutil JS Runtime is initialized, so defer it
                        var uriHelper = s.GetRequiredService<NavigationManager>();
                        return new HttpClient
                        {
                            BaseAddress = new Uri(uriHelper.BaseUri)
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
