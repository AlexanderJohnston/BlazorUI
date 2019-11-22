using System;

namespace BlazorUI.Client.Queries
{
    public class TimelineRoute
    {
        public Type QueryType { get; set; }
        public string Route { get; set; }

        public TimelineRoute(Type queryType, string route)
        {
            QueryType = queryType;
            Route = route;
        }

        /// <summary>
        ///   Required for deserialization.
        /// </summary>
        public TimelineRoute() { }
    }
}
