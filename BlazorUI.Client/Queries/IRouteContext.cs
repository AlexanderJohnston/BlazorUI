using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorUI.Client.Queries
{
    public interface IRouteContext
    {
        List<TimelineRoute> Map { get; set; }
    }
}
