using Blazor.Extensions;
using BlazorUI.Client;
using BlazorUI.Client.Queries;
using BlazorUI.Server.Native;
using BlazorUI.Server.Native.Win32NT;
using BlazorUI.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Totem.App.Web;
using Totem.Timeline.SignalR;
using Totem.Timeline.SignalR.Hosting;

namespace BlazorUI.Server
{
    public static class ServerExtensions
    {
        public static ConfigureWebApp EncryptionServices(this ConfigureWebApp configure)
        {
            return configure.Services((context, services) =>
            {
                //services.AddHostedService(er => new EncryptionReader(new DataProtection()));
            });
        }

        /// <summary>
        ///     Adds the <see cref="BlazorUI.Client"/> services for the web assembly UI to perform automatic two-way binding.
        /// </summary>
        /// <param name="configure">An instance of a <see cref="ConfigureWebApp"/> from Totem.</param>
        /// <param name="queryMap">A list of <see cref="TimelineRoute"/> which maps a Query type to its <see cref="HttpMethodAttribute"/> route.</param>
        /// <returns></returns>
        public static ConfigureWebApp ClientUIServices(this ConfigureWebApp configure, List<TimelineRoute> queryMap)
        {
            return configure.Services((context, services) =>
            {
                services.AddServerSideBlazor();
                services.AddSignalR().AddQueryNotifications();
                services.AddTransient<HubConnectionBuilder>();
                services.AddTransient<QueryController>();
                services.AddSingleton<IRouteContext, RouteContext>(sp => new RouteContext(queryMap));
                // Cannot seem to do this type of injection in client-side WASM right now...
                //services.AddTransient<IRouteContextFactory, RouteContextFactory>(sp => new RouteContextFactory(() => sp.GetService<IRouteContext>()));
                services.AddScoped<AppState>(state => new AppState(
                    state.GetRequiredService<QueryController>(),
                    state.GetRequiredService<HttpClient>()));
                services.AddHostedService(er => new EncryptionReader(new DataProtection()));
            });
        }

        /// <summary>
        ///     Adds the <see cref="BlazorUI.Client"/> files for the client as well as routing and the SignalR Query Hub.
        /// </summary>
        /// <param name="configure">An instance of <see cref="ConfigureWebApp"/> from Totem.</param>
        /// <returns></returns>
        public static ConfigureWebApp BlazorWebApplication(this ConfigureWebApp configure)
        {
            return configure.App(app =>
            {
                var environment = app.ApplicationServices
                    .GetRequiredService<IHostEnvironment>();
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
            });
        }
    }
}
