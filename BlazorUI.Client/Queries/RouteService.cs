using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Queries
{
    public class RouteService
    {
        public List<TimelineRoute> Map { get; set; }

        public RouteService(List<TimelineRoute> routes)
        {
            Map = routes;
        }
    }
}
