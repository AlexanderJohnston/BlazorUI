﻿using System;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Totem.App.Web;
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
using Totem.Timeline.Hosting;
using Totem.Timeline.SignalR;
using Totem.Timeline.Mvc.Hosting;
using Totem.Timeline.SignalR.Hosting;
using Serilog;
using BlazorUI.Client.Queries;
using BlazorUI.Server.PostSharp;
using BlazorUI.Server.Attributes;
using System.Collections.Generic;
using System.Text;
using BlazorUI.Shared;

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
            var queryMap = GetTimelineQueryEndpoints();
            //var routeService = new RouteService(queryMap);
            return WebApp.Run<ApplicationArea>(configuration
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
                    //services.AddOptions();
                    //services.Configure<AppConfig>(config => { config.Map = queryMap; });
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
                })
            );
        }

        public static List<TimelineRoute> GetTimelineQueryEndpoints()
        {
            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type));
            var actions = controllers
                    .SelectMany(type => type.GetMethods())
                    .Where(method => 
                        method.IsPublic 
                        && !method.IsDefined(typeof(NonActionAttribute))
                        && method.GetCustomAttributes()
                            .Any(attr => attr.GetType() == typeof(TimelineQueryAttribute)));
            var queries = new Dictionary<MethodInfo, TimelineQueryAttribute>();
            foreach(var action in actions)
            {
                queries.Add(action, action.GetCustomAttribute<TimelineQueryAttribute>());
            }
            var queryRoutes = queries.Select(query => 
                new TimelineRoute(query.Value.QueryType, ParseRouteFromAction(query.Key)));
            return queryRoutes.ToList();
        }

        public static string ParseRouteFromAction(MethodInfo action)
        {
            var sb = new StringBuilder();
            sb.Append('/');
            var controller = action.DeclaringType.Name.Replace("Controller", String.Empty);
            sb.Append(controller);
            sb.Append('/');
            sb.Append(action.Name);
            return sb.ToString();
        }
    }

}
