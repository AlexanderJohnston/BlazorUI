using BlazorUI.Client.Queries;
using BlazorUI.Server.Attributes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI.Server
{
    public class RouteContext : IRouteContext
    {
        public List<TimelineRoute> Map { get; set; }

        public RouteContext(List<TimelineRoute> map)
        {
            Map = map;
        }
    }
}
