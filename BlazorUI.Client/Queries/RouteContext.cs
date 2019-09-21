using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Queries
{
    public class RouteContext : IRouteContext
    {
        public IEnumerable<TimelineRoute> Map { get; set; }

        public RouteContext(IEnumerable<TimelineRoute> routes)
        {
            Map = routes;
        }
    }
}
