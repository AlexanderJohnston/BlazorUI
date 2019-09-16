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
using Totem.Timeline.Mvc.Hosting;
using Totem.Timeline.SignalR.Hosting;
using Serilog;
using BlazorUI.Server.PostSharp;

namespace BlazorUI.Server
{
    public class Program
    {
        public static Task Main()
        {
            // old method, working method
            //Type type = MethodBase.GetCurrentMethod().DeclaringType;
            //return WebApp.Run<CamArea>(Assembly.GetAssembly(type));
            var configuration = new ConfigureWebApp();
            return WebApp.Run<CamArea>(configuration
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
                .Serilog((context, logger) => {
                    logger = new LogConfiguration().VerboseLogger(logger);
                })
                .Services((context, services) =>
                {
                    services.AddServerSideBlazor();
                    services.AddSignalR().AddQueryNotifications();
                })
            );
        }
    }

}
