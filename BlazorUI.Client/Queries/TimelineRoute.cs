using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Queries
{
    public class TimelineRoute 
    {
        public string QueryType { get; set; }
        public string Route { get; set; }

        public TimelineRoute (Type queryType, string route)
        {
            QueryType = queryType.FullName;
            Route = route;
        }

        /// <summary>
        ///   Required for deserialization.
        /// </summary>
        public TimelineRoute() { }
    }
}
