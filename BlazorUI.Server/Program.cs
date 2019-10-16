using System;
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
using Microsoft.AspNetCore.Mvc.Routing;

namespace BlazorUI.Server
{
    public class Program
    {
        public static Task Main()
        {
            var configuration = new ConfigureWebApp();
            var queryMap = GetTimelineQueryEndpoints();

            return WebApp.Run<ApplicationArea>(configuration
                .BlazorWebApplication()
                .ClientUIServices(queryMap)
                .Serilog((context, logger) => {
                    logger = new LogConfiguration().VerboseLogger(logger);
                })
            );
        }

        /// <summary>
        ///     Reflects upon all <see cref="Controller"/> for routes marked by the <see cref="TimelineQueryAttribute"/>.
        ///     This is used to inform the client where it can request specific queries for SignalR subscription.
        /// </summary>
        /// <returns>A list of all <see cref="HttpMethodAttribute"/> routes which map back to a specific query.</returns>
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
