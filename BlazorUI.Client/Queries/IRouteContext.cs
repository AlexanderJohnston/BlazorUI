using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Queries
{
    public interface IRouteContext
    {
        IEnumerable<TimelineRoute> Map { get; set; }
    }
}
