using BlazorUI.Client.Queries;
using System.Collections.Generic;

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
