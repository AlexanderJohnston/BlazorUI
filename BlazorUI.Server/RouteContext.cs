using BlazorUI.Client.Queries;
using System.Collections.Generic;

namespace BlazorUI.Server
{
    /// <summary>
    ///     Provides a list of <see cref="TimelineRoute"/> to be used as a subscription context.
    /// </summary>
    public class RouteContext : IRouteContext
    {
        public List<TimelineRoute> Map { get; set; }

        public RouteContext(List<TimelineRoute> map)
        {
            Map = map;
        }
    }
}
